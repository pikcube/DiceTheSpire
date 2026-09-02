using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Common.Powers;
using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Uncommon;

public class SpookyNoises() : TheInventorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3), new PowerVar<ExhaustionPower>(2)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<ExhaustionPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card == this)
        {
            await PowerCmd.Apply<ExhaustionPower>(choiceContext, Owner.Creature, DynamicVars.Power<ExhaustionPower>().IntValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars.Power<ExhaustionPower>().UpgradeValueBy(-1);
    }

    public override string GetScrapId => nameof(BattleWrench);
}