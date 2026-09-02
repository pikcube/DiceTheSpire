using BaseLib.Abstracts;
using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Extensions;

namespace DiceTheSpire.DiceTheSpireCode.Powers;

public abstract class TheInventorPower : CustomPowerModel
{
    //Loads from TheInventor/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
}