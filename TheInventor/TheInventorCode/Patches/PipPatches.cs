using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Nodes.Cards;
using System.Data;
using System.Reflection;
using MegaCrit.Sts2.Core.Entities.Cards;
using TheInventor.TheInventorCode.Cards;

namespace TheInventor.TheInventorCode.Patches;

[HarmonyPatch(typeof(NCard), "UpdateEnergyCostVisuals")]
public static class PipPatches
{
    public static void Postfix(NCard __instance)
    {
        if (__instance.Model is not TheInventorCard c)
        {
            return;
        }

        FieldInfo energyLabelInfo = AccessTools.DeclaredField(typeof(NCard), "_energyLabel");
        MegaLabel l = (MegaLabel?) energyLabelInfo.GetValue(__instance) ?? throw new NoNullAllowedException();
        int withModifiers = c.EnergyCost.GetWithModifiers(CostModifiers.All);
        if (c.EnergyCost.CostsX)
        {
            l.SetTextAutoSize("X");
        }
        else if (withModifiers > 9)
        {
            l.SetTextAutoSize($"{withModifiers}");
        }
        else
        {
            l.SetTextAutoSize("");
        }

        FieldInfo energyTexture = AccessTools.DeclaredField(typeof(NCard), "_energyIcon");
        TextureRect r = (TextureRect?)energyTexture.GetValue(__instance) ?? throw new NoNullAllowedException();
        r.Texture = c.GetPips();
    }
}