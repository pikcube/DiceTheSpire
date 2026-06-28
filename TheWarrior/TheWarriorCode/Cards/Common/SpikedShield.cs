using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Common;


public class SpikedShield() : TheWarriorCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6M, BlockProps.card), new PowerVar<ThornsPower>(1M)];
    public override bool GainsBlock => true;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        if (CombatState is null)
        {
            return;
        }

        if (CombatState?.RoundNumber % 2 == 0 != IsUpgraded)
        {
            await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature, DynamicVars.Power<ThornsPower>().IntValue, Owner.Creature, this);
        }

        await base.OnPlay(choiceContext, cardPlay);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }

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


    protected override bool ShouldGlowGoldInternal => CombatState?.RoundNumber % 2 == 0 != IsUpgraded;

    protected override void OnUpgrade()
    {
        base.OnUpgrade();
        DynamicVars.Block.UpgradeValueBy(3);
    }

}

