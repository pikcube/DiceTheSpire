using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon;

public class PolarStar() : TheWarriorCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(11M, DamageProps.card), new RepeatVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        //ArgumentNullException.ThrowIfNull(Owner.Creature);

        if (CombatState is null)
        {
            return;
        }

        if (
           (CombatState.RoundNumber % 2 == 0 && IsUpgraded == false) ||
           (CombatState.RoundNumber % 2 != 0 && IsUpgraded == true)
           )
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
           .WithHitCount(DynamicVars.Repeat.IntValue)
           .FromCard(this)
           .Targeting(cardPlay.Target)
           .WithHitFx(VfxCmd.slashPath)
           .Execute(choiceContext);
        }
    }

    protected override bool IsPlayable => (
    (CombatState?.RoundNumber % 2 == 0 && IsUpgraded == false) ||
    (CombatState?.RoundNumber % 2 != 0 && IsUpgraded == true)
    );

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

    protected override bool ShouldGlowGoldInternal => this.IsPlayable;

    protected override void OnUpgrade()
    {
        base.OnUpgrade();
        this.DynamicVars.Damage.UpgradeValueBy(4);
    }

}
