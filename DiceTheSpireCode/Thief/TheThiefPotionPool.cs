using BaseLib.Abstracts;
using DiceTheSpire.DiceTheSpireCode.Common.Extensions;
using Godot;

namespace DiceTheSpire.DiceTheSpireCode.Thief;

public class TheThiefPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => TheThief.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}