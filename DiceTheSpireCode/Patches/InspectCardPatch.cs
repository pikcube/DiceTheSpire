using DiceTheSpire.DiceTheSpireCode.Inventor;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace DiceTheSpire.DiceTheSpireCode.Patches;

public static class InspectCardPatch
{
    public static Stack<bool> ShowStack { get; } = [];

    [HarmonyPatch(typeof(NInspectCardScreen), nameof(NInspectCardScreen.Open))]
    public static class InspectCardPatchOpen
    {
        public static void Prefix()
        {
            ShowStack.Push(TheInventorCard.EnableGadgetTipsGlobal);
            TheInventorCard.EnableGadgetTipsGlobal = true;
        }
    }

    [HarmonyPatch(typeof(NInspectCardScreen), nameof(NInspectCardScreen.Close))]
    public static class InspectCardPatchClose
    {
        public static void Postfix()
        {
            ShowStack.TryPop(out bool lastState);
            TheInventorCard.EnableGadgetTipsGlobal = lastState;
        }
    }
}