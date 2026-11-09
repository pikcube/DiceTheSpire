using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Thief.Rare;

public class SkeletonKey() : TheThiefCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState?.RunState.CurrentRoom is not CombatRoom combatRoom)
        {
            return;
        }

        CardSelectorPrefs prefs = new (CardSelectorPrefs.ExhaustSelectionPrompt, 1);
        IEnumerable<CardModel> cards = await CardSelectCmd.FromHand(choiceContext, Owner, prefs,
            (c => ModelDb.AllCharacterCardPools.Contains(c.Pool) || c.Pool is ColorlessCardPool), this);
        CardModel? card = cards.FirstOrDefault();
        if (card is null)
        {
            return;
        }
        await CardCmd.Exhaust(choiceContext, card);

        CardRarityOddsType odds;
        switch (combatRoom.RoomType)
        {
            case RoomType.Monster:
                odds = CardRarityOddsType.RegularEncounter;
                break;
            case RoomType.Elite:
                odds = CardRarityOddsType.EliteEncounter;
                break;
            case RoomType.Boss:
                odds = CardRarityOddsType.BossEncounter;
                break;
            default:
                odds = CardRarityOddsType.RegularEncounter;
                break;
        }
        combatRoom.AddExtraReward(Owner,
            new CardReward(new CardCreationOptions([card.Pool], CardCreationSource.Other, odds), 3, Owner));

        await PowerCmd.Apply<SkeletonKeyPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}