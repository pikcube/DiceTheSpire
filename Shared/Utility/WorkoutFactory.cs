using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Utility;

public static class WorkoutFactory
{
    private static IReadOnlyList<IWorkoutQuest> Workouts { get; } = [.. ModelDb.AllCards.OfType<IWorkoutQuest>()];
    public static PredeterminedCardReward CreateWorkoutReward(
        CardCreationSource source, 
        Player player, int count, 
        PlayerChoiceSynchronizer? synchronizer = null
        )
    {
        List<CardModel> quests =
        [
            .. Workouts.Where(w =>
                !player.Deck.Cards.Any(c =>
                    c.CanonicalInstance == w.AsCardModel() || c.CanonicalInstance == w.RewardCard))
                .Select(w => w.AsCardModel())
        ];

        player.PlayerRng.Rewards.Shuffle(quests);

        for (int n = 0; n < quests.Count; ++n)
        {
            CardModel card = quests[n];
            card = card.StrongMutableClone();
            card.Owner = null!;
            card.Owner = player;
            quests[n] = player.RunState.CloneCard(card);
        }

        List<CardModel> initial = quests[..count];
        List<CardModel> rest = quests[count..];

        return new PredeterminedCardReward(initial, source, player, rest, synchronizer);
    }
}