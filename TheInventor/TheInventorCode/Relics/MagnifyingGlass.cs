using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheInventor.TheInventorCode.Relics;


public class MagnifyingGlass : TheInventorRelic
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    public override RelicRarity Rarity => RelicRarity.Rare;

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if ((creator ?? card.Owner) == Owner)
        {
            
        }
    }
}