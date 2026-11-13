using DiceTheSpire.Inventor.Rare;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Thief.Uncommon;

public class BerlinKey() : TheThiefCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }
        CardSelectorPrefs prefs = new(CardSelectorPrefs.TransformSelectionPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
        if (card == null)
        {
            return;
        }

        TransformIntoAfterPlay = card.CreateClone();
    }

    public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources,
        CardLocation cardLocation)
    {
        if (card != this || !IsUpgraded || cardLocation.pileType == PileType.Exhaust)
        {
            return cardLocation;
        }

        return new CardLocation(cardLocation.player, PileType.Hand, CardPilePosition.Bottom);
    }
}