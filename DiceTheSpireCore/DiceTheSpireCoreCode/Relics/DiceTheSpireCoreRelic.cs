using BaseLib.Abstracts;
using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using Godot;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Relics
{
    public abstract class DiceTheSpireCoreRelic : CustomRelicModel
    {
        //DiceTheSpireCore/images/relics
        public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
        protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
        protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
    }
}