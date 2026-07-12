using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using TheInventor.TheInventorCode.Cards;

namespace TheInventor.TheInventorCode.Patches;

[HarmonyPatch(typeof(CardReward), "OnSelect")]
public class CardRewardToolTipPatch() : CustomSingletonModel(HookType.Run)
{
    public override bool TryModifyCardBeingAddedToDeck(CardModel card, out CardModel? newCard)
    {

        TheInventorCard.EnableTipsOnCards.Remove(card);

        newCard = card;
        return false;
    }

    public static void Prefix(CardReward __instance)
    {
        if (__instance.Player.Character is Character.TheInventor)
        {
            TheInventorCard.EnableTipsOnCards.AddRange(__instance.Cards);
        }
    }

    public static Task<bool> Postfix(Task<bool> __result, CardReward __instance)
    {
        return __result.ContinueWith(ContinuationAction);

        bool ContinuationAction(Task<bool> obj)
        {
            if (__instance.Player.Character is Character.TheInventor)
            {
                TheInventorCard.EnableTipsOnCards.RemoveAll(__instance.Cards.Contains);
            }


            return obj.Result;
        }
    }
}