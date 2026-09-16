using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Relics;

public class BagOfTricks : TheThiefRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner || Owner.PlayerCombatState?.TurnNumber != 1)
        {
            return;
        }
        IReadOnlyList<CardModel> cards =
        [
            .. ThiefHelperFunctions.GetDistinctOfClassForCombat(player,
                [.. ThiefHelperFunctions.BaseCharacterCardPools, .. ThiefHelperFunctions.DiceyNonThiefCardPools],
                DynamicVars.Cards.IntValue, player.RunState.Rng.CombatCardGeneration)
        ];
        Flash();
        CardModel? card = await CardSelectCmd.FromChooseACardScreen(
            choiceContext, cards, player);
        if (card is null)
        {
            return;
        }
        card.SetToFreeThisTurn();
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, player);
    }
}