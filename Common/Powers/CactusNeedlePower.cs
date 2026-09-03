using DiceTheSpire.Thief.Uncommon;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Common.Powers;

public class CactusNeedlePower : TemporaryThornsPower
{
    public override AbstractModel OriginModel => ModelDb.Card<CactusNeedle>();
}