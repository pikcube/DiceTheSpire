using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheInventor.TheInventorCode.Relics;

[UsedImplicitly]
public class Manual : TheInventorRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new CardsVar(1)];

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        return player == Owner ? amount + DynamicVars.Energy.IntValue : amount;
    }

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        return player == Owner ? count - DynamicVars.Cards.IntValue : count;
    }

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<Blueprint>();
    }
}