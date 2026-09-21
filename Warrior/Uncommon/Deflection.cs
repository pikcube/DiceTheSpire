using BaseLib.Extensions;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Uncommon;

public class Deflection() : TheWarriorCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12M, BlockProps.card), new PowerVar<DeflectionPower>(2M), new PowerVar<VigorPower>(2M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>(DynamicVars.Power<VigorPower>().IntValue), HoverTipFactory.FromPower<DeflectionPower>(DynamicVars.Power<DeflectionPower>().IntValue)];
    public override bool GainsBlock => true;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<DeflectionPower>(choiceContext, Owner.Creature, DynamicVars.Power<DeflectionPower>().IntValue, Owner.Creature, this);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
        DynamicVars.Power<VigorPower>().UpgradeValueBy(1);
        DynamicVars.Power<DeflectionPower>().UpgradeValueBy(1);
    }

}

