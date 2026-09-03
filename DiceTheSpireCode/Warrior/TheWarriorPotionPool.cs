using BaseLib.Abstracts;
using DiceTheSpire.DiceTheSpireCode.Common.Extensions;
using Godot;

namespace DiceTheSpire.DiceTheSpireCode.Warrior;

public class TheWarriorPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => TheWarrior.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}