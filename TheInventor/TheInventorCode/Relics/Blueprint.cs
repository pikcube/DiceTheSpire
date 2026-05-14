using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheInventor.TheInventorCode.Relics;


public class Blueprint : TheInventorRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2), new CardsVar(1)];

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        return player == Owner ? amount + DynamicVars.Energy.IntValue : amount;
    }

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        return player == Owner ? count - DynamicVars.Cards.IntValue : count;
    }
}