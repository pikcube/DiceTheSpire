using DiceTheSpireCore.DiceTheSpireCoreCode.Commands;
using DiceTheSpireCore.DiceTheSpireCoreCode.DynamicVars;
using DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Rare;
public class ParticleAccelerator() : TheWarriorCard(4, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy), IAfterRerollListener
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(1, DamageProps.card), .. RangeVars.Make(1, 4)];

    public override Task BeforeCombatStart()
    {
        DynamicVars.Damage.BaseValue = IsUpgraded ? 2 : 1;
        return Task.CompletedTask;
    }
    public Task AfterRerollAsync(CardModel card, bool isFixed, int originalCost, int getAmountToSpend, RerollDuration duration)
    {
        if (card == this)
        {
            DynamicVars.Damage.UpgradeValueBy(DynamicVars.Damage.IntValue);
        }
        return Task.CompletedTask;
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.PlayerCombatState is null || cardPlay.Target is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .WithHitCount(1)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }

}
