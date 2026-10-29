using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Warrior.Rare;

public class BicepCurl() : TheWarriorCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    //public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    public override bool HasTurnEndInHandEffect => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Nudge), HoverTipFactory.Static(BetterStaticHoverTips.Bump)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards.Where(c => !c.EnergyCost.CostsX && c.EnergyCost.GetWithModifiers(CostModifiers.None) > 0))
        {
            await NudgeCmd.NudgeAsync(card, NudgeDuration.UntilPlayed);
        }
    }
    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards)
        {
            await BumpCmd.BumpAsync(choiceContext, card);
        }
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}