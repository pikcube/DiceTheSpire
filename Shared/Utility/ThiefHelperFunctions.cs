using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;

namespace DiceTheSpire.Shared.Utility;

public static class ThiefHelperFunctions
{
    public static IEnumerable<CardModel> GetDistinctForCombatOfClass(Player player, IEnumerable<CharacterModel> characters, int count, Rng rng)
    {
        return characters.SelectMany(character => CardFactory.GetDistinctForCombat(player,
            character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint), 
            count, rng));
    }
}