using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using TheInventor.TheInventorCode.Extensions;

namespace TheInventor.TheInventorCode.Powers
{
    public abstract class TheInventorPower : CustomPowerModel
    {
        //Loads from TheInventor/images/powers/your_power.png
        public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
        public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
    }
}