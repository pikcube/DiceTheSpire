using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens;
using TheInventor.TheInventorCode.Cards;

namespace TheInventor.TheInventorCode.Utilities;

[HarmonyPatch(typeof(NInspectCardScreen), nameof(NInspectCardScreen.Open))]
public static class InspectCardPatchOpen
{
    public static void Prefix()
    {
        TheInventorCard.ShowGadgetTips = true;
    }
}

[HarmonyPatch(typeof(NInspectCardScreen), nameof(NInspectCardScreen.Close))]
public static class InspectCardPatchClose
{
    public static void Postfix()
    {
        TheInventorCard.ShowGadgetTips = false;
    }
}