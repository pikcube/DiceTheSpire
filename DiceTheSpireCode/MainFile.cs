using BaseLib.Utils;
using DiceTheSpire.DiceTheSpireCode.Inventor.Character;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace DiceTheSpire.DiceTheSpireCode;

//You're recommended but not required to keep all your code in this package and all your assets in the DiceTheSpire folder.
[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "DiceTheSpire"; //At the moment, this is used only for the Logger and harmony names.

    public static Logger Logger { get; } = new(ModId, LogType.Generic);
    public static string ResPath => $"res://{ModId}";

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();
        CustomLocTableManager.Register("gadgets.json");

        CustomCharacterUtils.TryOrderCustomCharacters<
            TheWarrior.TheWarriorCode.Character.TheWarrior,
            TheThief.TheThiefCode.Character.TheThief,
            TheInventor
        >();
    }
}