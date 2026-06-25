using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using TheInventor.TheInventorCode.Cards.Rare;

namespace TheInventor.TheInventorCode.Powers;

public class ResistorPower : TemporaryReducePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override AbstractModel OriginModel => ModelDb.Card<Resistor>();
}