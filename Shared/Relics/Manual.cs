using DiceTheSpire.Inventor;
using DiceTheSpire.Shared.Listeners;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;

namespace DiceTheSpire.Shared.Relics;

[UsedImplicitly]
public class Manual : TheInventorRelic, IModifyFlavorRelic
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

    public void ModifyFlavor(ref LocString locString)
    {
        CharacterStats? stats = SaveManager.Instance.Progress.GetStatsForCharacter(ModelDb.Character<TheInventor>().Id);
        int index = stats?.TotalWins ?? 0;
        switch (index)
        {
            case <=0:
                return;
            case < 5:
                locString = new LocString("relics", $"{Id.Entry}.flavor{index}");
                return;
            case >= 5:
                locString = new LocString("relics", $"{Id.Entry}.flavor5");
                return;
        }
    }
}