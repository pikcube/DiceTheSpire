using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Models;
using TheThief.TheThiefCode.Cards.Uncommon;

namespace TheThief.TheThiefCode.Powers;

public class CactusNeedlePower : TemporaryThornsPower
{
    public override AbstractModel OriginModel => ModelDb.Card<CactusNeedle>();
}