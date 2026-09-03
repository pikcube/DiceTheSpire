using BaseLib.Abstracts;
using DiceTheSpire.Common.Extensions;
using Godot;

namespace DiceTheSpire.Thief;

public class TheThiefRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => TheThief.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}