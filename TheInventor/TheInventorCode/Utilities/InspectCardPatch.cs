using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens;
using TheInventor.TheInventorCode.Cards;

namespace TheInventor.TheInventorCode.Utilities;

public static class InspectCardPatch
{
    public static Stack<bool> ShowStack { get; } = [];

    [HarmonyPatch(typeof(NInspectCardScreen), nameof(NInspectCardScreen.Open))]
    public static class InspectCardPatchOpen
    {
        public static void Prefix()
        {
            ShowStack.Push(TheInventorCard.EnableTipsGlobal);
            TheInventorCard.EnableTipsGlobal = true;
        }
    }

    [HarmonyPatch(typeof(NInspectCardScreen), nameof(NInspectCardScreen.Close))]
    public static class InspectCardPatchClose
    {
        public static void Postfix()
        {
            TheInventorCard.EnableTipsGlobal = ShowStack.Pop();
        }
    }
}