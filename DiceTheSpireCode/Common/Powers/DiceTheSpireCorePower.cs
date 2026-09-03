using BaseLib.Abstracts;
using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Common.Extensions;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;

public abstract class DiceTheSpireCorePower : CustomPowerModel
{
    //Loads from DiceTheSpireCore/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}