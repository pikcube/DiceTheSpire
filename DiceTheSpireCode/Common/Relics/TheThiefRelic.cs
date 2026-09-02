using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using DiceTheSpire.DiceTheSpireCode.Common.Extensions;
using DiceTheSpire.DiceTheSpireCode.Thief;

namespace DiceTheSpire.DiceTheSpireCode.Common.Relics;

[Pool(typeof(TheThiefRelicPool))]
public abstract class TheThiefRelic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}