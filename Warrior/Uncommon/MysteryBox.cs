using DiceTheSpire.Common.Commands;
using DiceTheSpire.Common.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Warrior.Uncommon;

public class MysteryBox() : TheWarriorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> cards = await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        foreach (CardModel card in cards)
        {
            if (card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
            {
                await RerollCmd.RerollAsync(card, RerollDuration.UntilEndOfTurnOrPlayed);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}