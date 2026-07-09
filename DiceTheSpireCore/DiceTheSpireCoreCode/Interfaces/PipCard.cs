using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface IPipCard
{
    public Texture2D GetPips(int? cost, bool isPretend, CardCostColor? energyCostColor = null);
    CardEnergyCost EnergyCost { get; }
}