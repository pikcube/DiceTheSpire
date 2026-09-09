using BaseLib.Abstracts;
using BaseLib.Extensions;
using DiceTheSpire.Shared.Extensions;

namespace DiceTheSpire.Shared.Powers;

public abstract class TheThiefPower : CustomPowerModel
{
    //Loads from TheThief/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}