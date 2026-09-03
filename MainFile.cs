using BaseLib.Utils;
using DiceTheSpire.Common.Utility;
using DiceTheSpire.Inventor;
using DiceTheSpire.Thief;
using DiceTheSpire.Warrior;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Pikcube.Common.Utility;
using SmartFormat;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace DiceTheSpire;

//You're recommended but not required to keep all your code in this package and all your assets in the DiceTheSpire folder.
[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "DiceTheSpire"; //At the moment, this is used only for the Logger and harmony names.

    public static Logger Logger { get; } = new(ModId, LogType.Generic);
    public static string ResPath => $"res://{ModId}";
    public static string ModPrefix { get; } = ModId.ToUpperInvariant();

    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        harmony.PatchAll();

        BetterHooks.ModifyCardText += RangeCardDescriptionModifier.ModifyCardText;

        CustomLocTableManager.Register("gadgets.json");


        LocAliasManager.Register(ModId, "cards", "cards.inventor", "cards.thief", "cards.warrior");
        LocAliasManager.Register(ModId, "relics", "relics.inventor", "relics.thief", "relics.warrior");
        LocAliasManager.Register(ModId, "potions", "potions.inventor", "potions.thief", "potions.warrior");
        LocAliasManager.Register(ModId, "powers", "powers.inventor", "powers.thief", "powers.warrior");
        
        CustomCharacterUtils.TryOrderCustomCharacters<
            TheWarrior,
            TheThief,
            TheInventor
        >();
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

[HarmonyPatch(typeof(ModManager), nameof(ModManager.GetModdedLocTables))]
public static class LocAliasManager
{
    private static List<LocAliasInfo> LocAliases { get; } = [];

    public static void Register(string modId, string path, params IEnumerable<string> aliases)
    {
        if (!path.EndsWith(".json"))
        {
            path = $"{path}.json";
        }

        DirAccess directory = DirAccess.Open(Path.Join($"res://{modId}", "localization"));

        string[] languages = directory.GetDirectories();

        string[] aliasArray = aliases as string[] ?? [.. aliases];

        foreach (string language in languages)
        {
            string basePath = string.Join('/', "res://localization", language, path);
            IEnumerable<string> aliasPaths = aliasArray
                .Select(s =>
                {
                    if (!s.EndsWith(".json"))
                    {
                        s = $"{s}.json";
                    }

                    return string.Join('/', $"res://{modId}", "localization", language, s);
                })
                .Where(s => ResourceLoader.Exists(s));

            LocAliasInfo? existing = LocAliases.SingleOrDefault(lai => lai.BasePath == basePath);

            if (existing is not null)
            {
                existing.AliasPaths.AddRange(aliasPaths);
            }
            else
            {
                LocAliases.Add(new LocAliasInfo(basePath, [.. aliasPaths]));
            }
        }
    }

    [HarmonyPostfix]
    internal static IEnumerable<string> MergeAliasesIntoTable(IEnumerable<string> __result, string language, string file)
    {
        string path = string.Join('/', "res://localization", language, file);
        foreach (string original in __result)
        {
            yield return original;
        }
        foreach (LocAliasInfo info in LocAliases.Where(lai => lai.BasePath == path))
        {
            foreach (string alias in info.AliasPaths)
            {
                yield return alias;
            }
        }
    }
}

internal record LocAliasInfo(string BasePath, List<string> AliasPaths);