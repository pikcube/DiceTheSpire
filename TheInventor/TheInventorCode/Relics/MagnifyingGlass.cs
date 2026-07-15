using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace TheInventor.TheInventorCode.Relics;


public class MagnifyingGlass : TheInventorRelic, IOnInspectListener
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    private bool IsReady { get; set; }
    public override Task BeforeCombatStart()
    {
        Status = RelicStatus.Active;
        InvokeDisplayAmountChanged();
        IsReady = true;
        return Task.CompletedTask;
    }

    public async Task OnInspectAsync(PlayerChoiceContext choiceContext, int cards, CardModel[] selectedCards, Player inspector)
    {
        if (!IsReady || inspector != Owner)
        {
            return;
        }

        await PlayerCmd.GainEnergy(1, Owner);
        Flash();
        Status = RelicStatus.Normal;
        InvokeDisplayAmountChanged();
        IsReady = false;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        Status = RelicStatus.Normal;
        InvokeDisplayAmountChanged();
        IsReady = false;
        return Task.CompletedTask;
    }
}