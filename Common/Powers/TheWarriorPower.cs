using BaseLib.Abstracts;
using BaseLib.Extensions;
using DiceTheSpire.Common.Extensions;
using Godot;

namespace DiceTheSpire.Common.Powers;

public abstract class TheWarriorPower : CustomPowerModel
{
    //Loads from TheWarrior/images/powers/your_power.png
    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
}