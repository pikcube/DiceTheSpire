using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using DiceTheSpire.Common.Extensions;
using DiceTheSpire.Thief;

namespace DiceTheSpire.Common.Relics;

[Pool(typeof(TheThiefRelicPool))]
public abstract class TheThiefRelic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}