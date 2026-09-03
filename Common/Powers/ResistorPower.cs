using BaseLib.Extensions;
using DiceTheSpire.Common.Extensions;
using DiceTheSpire.Inventor.Rare;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.Powers;

public class ResistorPower : TemporaryReducePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override AbstractModel OriginModel => ModelDb.Card<Resistor>();

    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}