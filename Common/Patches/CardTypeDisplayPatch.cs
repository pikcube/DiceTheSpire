using DiceTheSpire.Inventor.Token;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace DiceTheSpire.Common.Patches;

[HarmonyPatch(typeof(NCard), "UpdateTypePlaque")]
public class CardTypeDisplayPatch
{
    public static void Postfix(NCard __instance, MegaLabel ____typeLabel)
    {
        if (__instance.Model is not GadgetCard)
        {
            return;
        }
        ____typeLabel.SetTextAutoSize("Gadget");
    }
}