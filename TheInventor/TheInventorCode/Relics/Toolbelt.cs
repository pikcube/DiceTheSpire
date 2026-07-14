using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using Pikcube.Common.Utility;

namespace TheInventor.TheInventorCode.Relics;

public class Toolbelt : TheInventorRelic, IOnBlinkListener
{
    public override RelicRarity Rarity => RelicRarity.Common;
    private bool IsReady { get; set; }
    public override Task BeforeCombatStart()
    {
        Status = RelicStatus.Active;
        InvokeDisplayAmountChanged();
        IsReady = true;
        return Task.CompletedTask;
    }


    public async Task AfterCardBlinkedAsync(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (!IsReady || card.Owner != Owner)
        {
            return;
        }

        await CardPileCmd.Draw(choiceContext, Owner);
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