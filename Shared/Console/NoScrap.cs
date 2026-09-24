using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Players;

namespace DiceTheSpire.Shared.Console;

public class NoScrap : AbstractConsoleCmd
{
    public override CmdResult Process(Player? issuingPlayer, string[] args)
    {
        switch (args.First())
        {
            case "off":
                ScrapManager.DebugMode = ScrapManager.ScrapDebugMode.Off;
                return new CmdResult(true, "Scrap mode set to off");
            case "optional":
                ScrapManager.DebugMode = ScrapManager.ScrapDebugMode.Optional;
                return new CmdResult(true, "Scrap mode set to optional");
            default:
                ScrapManager.DebugMode = ScrapManager.ScrapDebugMode.Default;
                return new CmdResult(true, "Scrap mode set to default");
        }
    }

    public override string CmdName => "pikcube.scrap";
    public override string Args => "[default|optional|off]";
    public override string Description => "Modify Scrapping Behavior";
    public override bool IsNetworked => true;
}