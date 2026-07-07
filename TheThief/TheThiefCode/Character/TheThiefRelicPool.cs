using BaseLib.Abstracts;
using Godot;
using TheThief.TheThiefCode.Extensions;

namespace TheThief.TheThiefCode.Character;

public class TheThiefRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => TheThief.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}