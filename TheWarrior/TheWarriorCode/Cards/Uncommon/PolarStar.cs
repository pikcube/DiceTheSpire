using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon;

public class PolarStar() : TheWarriorCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10M, DamageProps.card), new RepeatVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        //ArgumentNullException.ThrowIfNull(Owner.Creature);

        if (CombatState is null)
        {
            return;
        }

        if (IsPlayable)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
           .WithHitCount(DynamicVars.Repeat.IntValue)
           .FromCard(this, cardPlay)
           .Targeting(cardPlay.Target)
           .WithHitFx(VfxCmd.slashPath)
           .Execute(choiceContext);
        }
    }

    //We want the left hand side to be true when downgraded and false when upgraded, so this check ends up just being an exclusive or
    /* Original Code
     * (CombatState.RoundNumber % 2 == 0 && !IsUpgraded) ||
     * (CombatState.RoundNumber % 2 != 0 && IsUpgraded)
     */
    protected override bool IsPlayable => CombatState?.RoundNumber % 2 == 0 != IsUpgraded;


    //protected override bool IsPlayable
    //{
    //    get
    //    {
    //        if (CombatState is null)
    //        {
    //            return true;
    //        }
    //        return (CombatState.RoundNumber % 2 == 0);
    //    }
    //}
    //

    protected override bool ShouldGlowGoldInternal => IsPlayable;

    protected override void OnUpgrade()
    {
        base.OnUpgrade();
        DynamicVars.Damage.UpgradeValueBy(3);
    }

}
