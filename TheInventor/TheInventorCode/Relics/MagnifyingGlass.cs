using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheInventor.TheInventorCode.Relics;


public class MagnifyingGlass : TheInventorRelic
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    public override RelicRarity Rarity => RelicRarity.Rare;
}