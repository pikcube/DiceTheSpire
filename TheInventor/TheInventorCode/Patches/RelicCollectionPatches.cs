using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;

namespace TheInventor.TheInventorCode.Patches;

[HarmonyPatch(typeof(NRelicCollectionCategory), nameof(NRelicCollectionCategory.LoadRelics))]
internal static class RelicCollectionPatches
{
    public static void Prefix(RelicRarity relicRarity)
    {
        if (relicRarity == RelicRarity.Starter)
        {
            Character.TheInventor.HideGadget = true;
        }
    }

    public static void Postfix()
    {
        Character.TheInventor.HideGadget = false;
    }

}