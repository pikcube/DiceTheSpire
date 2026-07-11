using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Cards.Token;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Utilities;

namespace TheInventor.TheInventorCode.Patches;

[HarmonyPatch(typeof(InfestedAutomaton), "GenerateInitialOptions")]
public class InfestedAutomatonPatch
{
    public static IReadOnlyList<EventOption> Postfix(IReadOnlyList<EventOption> __result, InfestedAutomaton __instance)
    {
        if (__instance.Owner?.Character is not Character.TheInventor inventor)
        {
            return __result;
        }

        GadgetCard newGadget = GadgetCard1.Create().StrongMutableClone();
        newGadget.SetVars(ScrapManager.AllGadgets[nameof(HeatRay)]);

        List<EventOption> options =
        [
            ..__result.Where(option => option.TextKey != "INFESTED_AUTOMATON.pages.INITIAL.options.TOUCH_CORE"),
            new(__instance, () => OnChosen(__instance, __instance.Owner, inventor), "THEINVENTOR-INFESTED_AUTOMATON.pages.INITIAL.options.SCRAP_CORE", new CardHoverTip(newGadget))
        ];
        return options.AsReadOnly();
    }

    private static async Task OnChosen(InfestedAutomaton instance, Player p, Character.TheInventor _)
    {
        ScrapManager.GadgetId.Set(p, nameof(HeatRay));

        AccessTools.DeclaredMethod(typeof(EventModel), "SetEventFinished").Invoke(instance,
        [
            new LocString(instance.LocTable, "THEINVENTOR-INFESTED_AUTOMATON.SCRAP_CORE.description")
        ]);

        await GadgetCard1.ShowAsync(ScrapManager.AllGadgets[nameof(HeatRay)]);
    }
}