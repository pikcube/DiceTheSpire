using BaseLib.Config;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace TheInventor.TheInventorCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "TheInventor"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();

        CustomLocTableManager.Register("gadgets.json");

        ModConfigRegistry.Register(ModId, new InventorDebugConfig());
    }
}

public class InventorDebugConfig : SimpleModConfig
{
    public static bool ShowGadgetTips { get; set; } = true;
}