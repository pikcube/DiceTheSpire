using BaseLib.Abstracts;
using DiceTheSpire.DiceTheSpireCode.Common.Extensions;
using Godot;

namespace DiceTheSpire.DiceTheSpireCode.Inventor;

public class TheInventorRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => TheInventor.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}