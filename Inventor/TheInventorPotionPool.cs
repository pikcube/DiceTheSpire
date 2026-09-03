using BaseLib.Abstracts;
using DiceTheSpire.Common.Extensions;
using Godot;

namespace DiceTheSpire.Inventor;

public class TheInventorPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => TheInventor.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}