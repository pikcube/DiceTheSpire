using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;

namespace DiceTheSpire.DiceTheSpireCode.Patches;

[HarmonyPatch(typeof(CardReward), nameof(CardReward.Reroll))]
public static class CardRewardRerollPatch
{
    public static Dictionary<CardReward, List<CardModel>> HijackMap { get; set; } = [];

    public static void HijackReroll(CardReward cardReward, List<CardModel> backupCards)
    {
        RunManager.Instance.RoomEntered -= InstanceOnRoomEntered;
        RunManager.Instance.RoomEntered += InstanceOnRoomEntered;
        HijackMap[cardReward] = backupCards;  
    }

    private static void InstanceOnRoomEntered()
    {
        RunManager.Instance.RoomEntered -= InstanceOnRoomEntered;
        HijackMap.Clear();
    }

    public static bool Prefix(CardReward __instance)
    {
        if (!HijackMap.TryGetValue(__instance, out List<CardModel>? cards))
        {
            return true;
        }

        __instance.CanReroll = false;
        PrivatePropertyWrapper<CardReward, List<CardCreationResult>> privatePropertyWrapper = __instance.PrivatePropertyWrapper<CardReward, List<CardCreationResult>>("_cards");
        foreach (CardCreationResult card in privatePropertyWrapper.Value ?? [])
        {
            __instance.Player.RunState.CurrentMapPointHistoryEntry?.GetEntry(__instance.Player.NetId).CardChoices.Add(new CardChoiceHistoryEntry(card.Card, false));
        }


        AccessTools.DeclaredField(typeof(CardReward), "_hasBeenRerolled").SetValue(__instance, true);
        (privatePropertyWrapper.Value ?? []).Clear();

        List<CardCreationResult> results = [.. cards.Select(c => new CardCreationResult(c))];

        (privatePropertyWrapper.Value ?? []).AddRange(results);
        __instance.Populate();

        HijackMap.Remove(__instance);

        return false;
    }
}