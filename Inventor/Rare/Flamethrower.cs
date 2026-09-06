using BaseLib.Extensions;
using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Rare;


public class Flamethrower() : TheInventorCard(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    public override string GetScrapId => nameof(Blowtorch);
    public const string Judge = "JudgeVar";

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12, DamageProps.card), new(Judge, 12)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState)
            .WithValueProp(DynamicVars.Damage.Props)
            .BeforeDamage(() =>
            {
                foreach (Creature c in CombatState.Enemies.ToArray())
                {
                    NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireBurningVfx.Create(c, 1, true));
                }

                return Task.CompletedTask;
            })
            .Execute(choiceContext);

        foreach (Creature c in CombatState.Enemies.Where(c => c.CurrentHp <= DynamicVars[Judge].IntValue).ToArray())
        {
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(c));
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireBurstVfx.Create(c, 1));
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(c));
            await CreatureCmd.Kill(c);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(2);
        DynamicVars.Damage.UpgradeValueBy(18);
        DynamicVars[Judge].UpgradeValueBy(18);
    }
}