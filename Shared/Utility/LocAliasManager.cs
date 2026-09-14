using System.Data;
using System.Text.Json;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using FileAccess = Godot.FileAccess;

namespace DiceTheSpire.Shared.Utility;

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

    public static void LoadJson(string modId)
    {
        string jsonString = FileAccess.GetFileAsString(GetAllFilesRecursive($"res://{modId}").Single(f => f.EndsWith("locAliases.json")));
        LocAliasInfo[] locInfos = JsonSerializer.Deserialize<LocAliasInfo[]>(jsonString) ?? throw new NoNullAllowedException();
        foreach (LocAliasInfo locInfo in locInfos)
        {
            Register(modId, locInfo.BasePath, locInfo.AliasPaths);
        }
    }

    private static IEnumerable<string> GetAllFilesRecursive(string directoryPath)
    {
        using DirAccess dir = DirAccess.Open(directoryPath);
        foreach (string file in dir.GetFiles())
        {
            yield return $"{directoryPath}/{file}";
        }

        foreach (string subDirectory in dir.GetDirectories())
        {
            foreach (string file in GetAllFilesRecursive($"{directoryPath}/{subDirectory}"))
            {
                yield return file;
            }
        }
    }
}