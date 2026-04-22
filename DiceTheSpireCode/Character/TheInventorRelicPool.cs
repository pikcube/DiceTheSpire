using BaseLib.Abstracts;
using DiceTheSpire.DiceTheSpireCode.Extensions;
using Godot;

namespace DiceTheSpire.DiceTheSpireCode.Character
{
    public class TheInventorRelicPool : CustomRelicPoolModel
    {
        public override Color LabOutlineColor => TheInventor.Color;

        public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
        public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    }
}