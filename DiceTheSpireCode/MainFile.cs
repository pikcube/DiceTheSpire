using BaseLib.Utils;
using DiceTheSpire.DiceTheSpireCode.Inventor;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using System.Text.Json;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace DiceTheSpire.DiceTheSpireCode;

//You're recommended but not required to keep all your code in this package and all your assets in the DiceTheSpire folder.
[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public static List<LocAliasInfo> LocAliases { get; } = [];

    public const string ModId = "DiceTheSpire"; //At the moment, this is used only for the Logger and harmony names.

    public static Logger Logger { get; } = new(ModId, LogType.Generic);
    public static string ResPath => $"res://{ModId}";
    public static string ModPrefix { get; } = StringHelper.Slugify(ModId);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        harmony.PatchAll();
        
        
        CustomLocTableManager.Register("gadgets.json");

        
        RegisterMerge("cards.json", "cards.inventor.json");
        RegisterMerge("relics.json", "relics.inventor.json");
        RegisterMerge("potions.json", "potions.inventor.json");
        RegisterMerge("powers.json", "powers.inventor.json");
        
        CustomCharacterUtils.TryOrderCustomCharacters<
            TheWarrior.TheWarriorCode.Character.TheWarrior,
            TheThief.TheThiefCode.Character.TheThief,
            TheInventor
        >();
    }

    private static void RegisterMerge(string path, params IEnumerable<string> aliases)
    {
        DirAccess directory = DirAccess.Open(Path.Join(ResPath, "localization"));

        string[] languages = directory.GetDirectories();

        string[] aliasArray = aliases as string[] ?? [.. aliases];

        foreach (string language in languages)
        {
            string basePath = string.Join('/', ResPath, "localization", language, path);
            IEnumerable<string> aliasPaths = aliasArray.Select(s => string.Join('/', ResPath, "localization", language, s));

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
}

[HarmonyPatch(typeof(LocManager), "LoadTable")]
public static class MergeTablePatch
{
    [HarmonyPostfix]
    public static void MergeAliasesIntoTable(ref Dictionary<string, string> __result, string path)
    {
        foreach (LocAliasInfo info in MainFile.LocAliases.Where(lai => lai.BasePath == path))
        {
            foreach (string alias in info.AliasPaths)
            {
                using Godot.FileAccess? fileAccess = Godot.FileAccess.Open(alias, Godot.FileAccess.ModeFlags.Read);
                if (fileAccess is null)
                {
                    continue;
                }

                string json = fileAccess.GetAsText();
                Dictionary<string, string>? newDict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (newDict is null)
                {
                    continue;
                }

                foreach ((string key, string value) in newDict)
                {
                    __result[key] = value;
                }
            }
        }
    }
}

public record LocAliasInfo(string BasePath, List<string> AliasPaths);