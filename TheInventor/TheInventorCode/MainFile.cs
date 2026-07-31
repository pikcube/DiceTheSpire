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

        //Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());

        CustomLocTableManager.Register("gadgets.json");
    }
}

/*
public partial class GadgetIcon : Control
{
    public static AddedNode<NTopBar, GadgetIcon> Node = new(topbar =>
    {
        GadgetIcon gadgetIcon = new();
        
        Texture2D tex = ResourceLoader.Load<Texture2D>($"{MainFile.ResPath}/images/relics/gadget.png");
        Vector2 size = tex.GetSize();

        TextureRect texRect = new TextureRect();
        texRect.Name = tex.ResourcePath;
        texRect.Size = new Vector2(50, 50);
        texRect.Texture = tex;
        texRect.PivotOffset = size / 2f;
        texRect.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
        texRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
        texRect.MouseFilter = MouseFilterEnum.Ignore;

        gadgetIcon.Size = new Vector2(50, 50);
        gadgetIcon.Position = new Vector2(10, 10);
        gadgetIcon.AddChild(texRect);

        topbar.AddChild(gadgetIcon);

        return gadgetIcon;
    });
}
*/