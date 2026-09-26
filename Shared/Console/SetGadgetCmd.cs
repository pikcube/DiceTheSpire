using DiceTheSpire.Inventor;
using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Console;

public class SetGadgetCmd : AbstractConsoleCmd
{
    public override CmdResult Process(Player? issuingPlayer, string[] args)
    {
        string gadgetId = args.First();
        GadgetModel? gadget = ModelDb.AllModels.OfType<GadgetModel>().SingleOrDefault(g => g.GadgetId == gadgetId);
        if (gadget is null)
        {
            return new CmdResult(false, $"{gadgetId} not found");
        }

        if (issuingPlayer?.Character is not TheInventor)
        {
            return new CmdResult(false, "Gadgets only work on The Inventor");
        }

        ScrapManager.SetGadgetInfo(issuingPlayer, gadgetId, false);

        return new CmdResult(true, "Gadget set");
    }

    public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
    {
        if (args.Length >= 1)
        {
            return new CompletionResult()
            {
                Type = CompletionType.Argument,
                ArgumentContext = CmdName
            };
        }

        string[] candidates = [.. ModelDb.AllModels.OfType<GadgetModel>().Select(g => g.GadgetId)];
        return CompleteArgument(candidates, [], args.FirstOrDefault() ?? "");
    }

    public override string CmdName => "pikcube.setgadget";
    public override string Args => "<id:string>";
    public override string Description => "Set the gadget of the specified player.";
    public override bool IsNetworked => true;
}