using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Uncommon;


public class LeatherArmor() : TheInventorCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        //new BlockVar(4, BlockProps.card), 
        new PowerVar<ReducePower>(2)
    ];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<ReducePower>(DynamicVars.Power<ReducePower>().IntValue)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        //await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<ReducePower>(choiceContext, Owner.Creature, DynamicVars.Power<ReducePower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        //DynamicVars.Block.UpgradeValueBy(3);
        DynamicVars.Power<ReducePower>().UpgradeValueBy(1);
    }

    public override string GetScrapId => nameof(Protection);
}