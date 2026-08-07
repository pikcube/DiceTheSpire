using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheThief.TheThiefCode.Cards.Uncommon;

public class BerlinKey() : TheThiefCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }
        CardSelectorPrefs prefs = new(DiceySelection.ToDupe, 1);
        CardModel? original = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
        if (original == null)
        {
            return;
        }
        
        CardPileAddResult addedCard =  await CardPileCmd.AddGeneratedCardToCombat(original.CreateClone(), IsUpgraded ? PileType.Hand : PileType.Discard, Owner);
        CardCmd.PreviewCardPileAdd(addedCard);
    }
}