using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers.Models;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.Interfaces;

public interface IPipCard
{
    public Texture2D GetPips(int? cost, bool isPretend, CardCostColor? energyCostColor = null);
    CardEnergyCost EnergyCost { get; }
}

public static class PipCard
{
    public static Texture2D GetPipsForMod<T>(T card, string resPath, int? cost, bool isPretend, CardCostColor? energyCostColor = null) where T : CardModel, IPipCard
    {
        string costText = cost switch
        {
            null => "X",
            < 1 or > 9 => "0",
            _ => $"{cost}"
        };

        if (card.EnergyCost is { CostsX: false, WasJustUpgraded: true })
        {
            return ResourceLoader.Load<Texture2D>(EnergyDiceconPath(resPath, "Green", $"ui_dice_dice{costText}.png"));
        }

        energyCostColor ??= CardCostHelper.GetEnergyCostColor(card, card.CombatState);
        switch (energyCostColor)
        {
            case CardCostColor.Unmodified:
                return ResourceLoader.Load<Texture2D>(EnergyDiceconPath(resPath, $"ui_dice_dice{costText}.png"));
            case CardCostColor.Increased:
                return ResourceLoader.Load<Texture2D>(EnergyDiceconPath(resPath, "Blue", $"ui_dice_dice{costText}.png"));
            case CardCostColor.Decreased:
                return ResourceLoader.Load<Texture2D>(EnergyDiceconPath(resPath,"Green", $"ui_dice_dice{costText}.png"));
            case CardCostColor.InsufficientResources when !isPretend:
                return ResourceLoader.Load<Texture2D>(EnergyDiceconPath(resPath, "Red", $"ui_dice_dice{costText}.png"));
            default:
                goto case CardCostColor.Unmodified;
        }
    }

    private static string EnergyDiceconPath(string resPath, params ReadOnlySpan<string?> path)
    {
        return Path.Join([resPath, "images", "charui", "Energy", .. path]);
    }
}