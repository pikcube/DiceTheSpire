using BaseLib.Abstracts;
using Godot;
using TheInventor.TheInventorCode.Extensions;

namespace TheInventor.TheInventorCode.Character
{
    public class TheInventorPotionPool : CustomPotionPoolModel
    {
        public override Color LabOutlineColor => TheInventor.Color;


        public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
        public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    }
}