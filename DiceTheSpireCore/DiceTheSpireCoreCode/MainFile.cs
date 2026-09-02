using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Pikcube.Common.Utility;
using SmartFormat;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace DiceTheSpireCore.DiceTheSpireCoreCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "DiceTheSpireCore"; //Used for resource filepath
    public static string ModPrefix { get; } = StringHelper.Slugify(ModId);
    public const string ResPath = $"res://{ModId}";

    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();

        BetterHooks.ModifyCardText += RangeCardDescriptionModifier.ModifyCardText;
    }
}

[HarmonyPatch(typeof(LocManager), "LoadLocFormatters")]
public static class StringFormatterPatches
{
    public static void Postfix()
    {
        Smart.Default.AddExtensions(new DiceIconFormatter());
    }
}