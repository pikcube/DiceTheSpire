using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;

namespace DiceTheSpire.Shared.Utility;

/// <summary>
/// Special case of CardReward where both the cards to offer and the cards to offer if rerolled are set in the constructor.
/// </summary>
/// <param name="cardsToOffer">List of cards to offer as rewards.</param>
/// <param name="source">The source of the reward.</param>
/// <param name="player">The player that this reward is for.</param>
/// <param name="cardsToOfferOnReroll">If the options are rerolled with <see cref="T:MegaCrit.Sts2.Core.Models.Relics.Driftwood" />, then these cards wll be shown instead.</param>
/// <param name="synchronizer">PlayerChoiceSynchronizer, for injection in tests.</param>
[HarmonyPatch]
public class PredeterminedCardReward(IEnumerable<CardModel> cardsToOffer, CardCreationSource source, Player player, IEnumerable<CardModel> cardsToOfferOnReroll, PlayerChoiceSynchronizer? synchronizer = null) 
    : CardReward(cardsToOffer, source, player, new CardCreationOptions([], source, CardRarityOddsType.Uniform), synchronizer)
{
    public IEnumerable<CardModel> RerollCards { get; } = cardsToOfferOnReroll;

    [HarmonyReversePatch]
    [HarmonyPatch(typeof(CardReward), nameof(Populate))]
    public override void Populate()
    {
        _ = Transpiler(null!);
        return;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher matcher = new(instructions);

            //Replace call to CreateForReward with call to SetRerollOptions
            matcher.MatchStartForward(CodeMatch.Calls(() => CardFactory.CreateForReward(null!, 0, null!)))
                .ThrowIfInvalid("Cannot find where card creation results are invoked")
                .RemoveInstruction()
                .InsertAndAdvance(CodeInstruction.LoadArgument(0))
                .InsertAndAdvance(CodeInstruction.Call(() => SetRerollOptions(0, null!)));

            //Remove load of card creation options since it is unused
            matcher.MatchStartBackwards(CodeMatch.WithOpcodes([OpCodes.Ldloc_0]))
                .ThrowIfInvalid("Could not find loading of options")
                .RemoveInstruction();

            //Remove call to get_player since it is unused
            matcher.MatchStartBackwards(CodeMatch.Calls(typeof(Reward).DeclaredProperty(nameof(Player)).GetMethod))
                .ThrowIfInvalid("Could not find call to get player")
                .RemoveInstruction();

            //Remove load of this that is consumed by get_player
            matcher.MatchStartBackwards(CodeMatch.WithOpcodes([OpCodes.Ldarg_0]))
                .ThrowIfInvalid("Could not find load of this")
                .RemoveInstruction();

            return matcher.Instructions();
        }
    }

    public static IEnumerable<CardCreationResult> SetRerollOptions(int cardCount, PredeterminedCardReward instance)
    {
        return instance.RerollCards.Take(cardCount).Select(c => new CardCreationResult(c));
    }
}