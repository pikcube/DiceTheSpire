using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Relics;

[UsedImplicitly]
public class Gadget : TheInventorRelic
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [LinkedGadget.GadgetName, LinkedGadget.GadgetDescription];

    private static IEnumerable<AbstractModel> CombatStateDelegates(CombatState combatState)
    {
        return combatState.Players.SelectMany(p => p.Relics.OfType<Gadget>()).Select(g => g.LinkedGadget);
    }

    private static IEnumerable<AbstractModel> RunStateDelegates(RunState runState)
    {
        return runState.Players.SelectMany(p => p.Relics.OfType<Gadget>()).Select(g => g.LinkedGadget);
    }

    static Gadget()
    {
        ModHelper.SubscribeForCombatStateHooks("TheInventor.Gadgets", CombatStateDelegates);
        ModHelper.SubscribeForRunStateHooks("TheInventor.Gadgets", RunStateDelegates);
    }

    public override RelicModel? GetUpgradeReplacement()
    {
        return ModelDb.Relic<Gadget>();
    }

    public static Dictionary<string, AbstractGadget> AllGadgets { get; } = [];
    
    public override RelicRarity Rarity => RelicRarity.Starter;

    [SavedProperty] 
    public string GadgetId { get; set; } = "Gadget.HeatRay";

    public AbstractGadget LinkedGadget
    {
        get
        {
            if (field?.GadgetId != GadgetId || field.Parent != this)
            {
                field = AllGadgets[GadgetId].GetMutable(this);
            }
            return field;
        }
        private set;
    }
}