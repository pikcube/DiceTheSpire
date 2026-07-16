using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using Pikcube.Common.Extensions;

namespace TheInventor.TheInventorCode.Relics;


public class LoudBirdsMixtape : TheInventorRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    public override Task BeforeCombatStart()
    {
        StrengthPower? strength = Owner.Creature.GetPower<StrengthPower>();
        if (strength is null || strength.Amount >= 0)
        {
            Status = RelicStatus.Normal;
        }
        else
        {
            Status = RelicStatus.Active;
        }
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner)
        {
            return;
        }

        StrengthPower? strength = Owner.Creature.GetPower<StrengthPower>();
        if (strength is null || strength.Amount >= 0)
        {
            return;
        }

        Flash();
        await StrengthPower.ApplyAsync(choiceContext, Owner.Creature, 1, Owner.Creature, null);
    }

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        StrengthPower? strength = Owner.Creature.GetPower<StrengthPower>();
        if (strength is null || strength.Amount >= 0)
        {
            Status = RelicStatus.Normal;
        }
        else
        {
            Status = RelicStatus.Active;
        }
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }
}