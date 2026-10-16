using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;

public abstract class NextTurnPower : DiceTheSpirePower, IHasSecondAmount
{
    public abstract TickTime Tick { get; }
    public abstract CombatSide TickSide { get; }
    protected virtual IEnumerable<DynamicVar> ExtraCanonicalVars => [];
    protected sealed override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Delay", 1), .. ExtraCanonicalVars];
    public string GetSecondAmount() => $"{DynamicVars["Delay"].IntValue}";

    public sealed override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public virtual PowerInstanceType NextTurnInstanceType => PowerInstanceType.InstancedPerApplier;


    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == TickSide && Tick == TickTime.Start)
        {
            HookPlayerChoiceContext context = new(this, LocalContext.NetId ?? 0, CombatState, GameActionType.Combat);
            await TickDownAsync(context);
        }
    }

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == TickSide && Tick == TickTime.End)
        {
            await TickDownAsync(choiceContext);
        }
    }

    private async Task TickDownAsync(PlayerChoiceContext choiceContext)
    {
        if (DynamicVars["Delay"].IntValue == 1)
        {
            await OnDelayOverAsync(choiceContext);
            await PowerCmd.Remove(this);
        }
        else
        {
            --DynamicVars["Delay"].BaseValue;
            this.InvokeSecondAmountChanged();
        }
    }

    public abstract Task OnDelayOverAsync(PlayerChoiceContext choiceContext);
}

public enum TickTime
{
    None = 0,
    Start = 1,
    End = 2,
}

public static class NextTurnPowerExtensions
{
    extension<T>(T) where T : NextTurnPower
    {
        public static async Task ApplyAsync(PlayerChoiceContext choiceContext,
            IEnumerable<Creature> targets,
            decimal amount,
            int delay,
            Creature? applier,
            CardModel? cardSource,
            bool silent = false)
        {
            foreach (Creature c in targets)
            {
                await ApplyAsync<T>(choiceContext, c, amount, delay, applier, cardSource, silent);
            }
        }

        public static async Task ApplyAsync(PlayerChoiceContext choiceContext,
            Creature target,
            decimal amount,
            int delay,
            Creature? applier,
            CardModel? cardSource,
            bool silent = false)
        {
            T? existing = target.Powers.OfType<T>().FirstOrDefault(p => p.DynamicVars["Delay"].IntValue == delay && p.Applier == applier);
            if (existing is not null && existing.NextTurnInstanceType == PowerInstanceType.InstancedPerApplier)
            {
                await PowerCmd.ModifyAmount(choiceContext, existing, amount, applier, cardSource, silent);
            }
            else
            {
                T? power = await PowerCmd.Apply<T>(choiceContext, target, amount, applier, cardSource, silent);
                power?.DynamicVars["Delay"].BaseValue = delay;
                power?.InvokeSecondAmountChanged();
            }
        }
    }
}