using BaseLib.Abstracts;
using Godot;
using TheWarrior.TheWarriorCode.Extensions;

namespace TheWarrior.TheWarriorCode.Character
{
    public class TheWarriorRelicPool : CustomRelicPoolModel
    {
        public override Color LabOutlineColor => TheWarrior.Color;

        public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
        public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    }
}