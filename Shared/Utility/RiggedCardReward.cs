using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;

namespace DiceTheSpire.Shared.Utility;

[HarmonyPatch]
public class RiggedCardReward(IEnumerable<CardModel> cardsToOffer, CardCreationSource source, Player player, IEnumerable<CardModel> cardsToOfferOnReroll, PlayerChoiceSynchronizer? synchronizer = null) 
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
            matcher.MatchStartForward(CodeMatch.Calls(() => CardFactory.CreateForReward(null!, 0, null!)))
                .ThrowIfInvalid("Cannot find where card creation results are invoked")
                .RemoveInstruction()
                .InsertAndAdvance(CodeInstruction.LoadArgument(0))
                .InsertAndAdvance(CodeInstruction.Call(() => SetRerollOptions(null!, 0, null!, null!)));

            return matcher.Instructions();
        }
    }

    public static IEnumerable<CardCreationResult> SetRerollOptions(Player player, int cardCount, CardCreationOptions options, RiggedCardReward instance)
    {
        _ = player;
        _ = options;
        return instance.RerollCards.Take(cardCount).Select(c => new CardCreationResult(c));
    }
}