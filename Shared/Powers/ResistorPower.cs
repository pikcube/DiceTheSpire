using DiceTheSpire.Inventor.Uncommon;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;

public class ResistorPower : TemporaryReducePower
{
    public override AbstractModel OriginModel => ModelDb.Card<Resistor>();
}