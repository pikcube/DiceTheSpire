using DiceTheSpire.DiceTheSpireCode.Common.Commands;
using DiceTheSpire.DiceTheSpireCode.Common.Listeners;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace DiceTheSpire.DiceTheSpireCode.Common.Powers;

public class BicepCurlPower : DiceTheSpireCorePower, IAfterNudgeListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public async Task AfterNudgeAsync(CardModel card, int originalCost, int getAmountToSpend, NudgeDuration duration)
    {
        if (Owner.Player?.PlayerCombatState is null)
        {
            return;
        }
        PlayerCombatState pcs = Owner.Player.PlayerCombatState;
        CardModel[] allCards = [.. pcs.Hand.Cards, .. pcs.DrawPile.Cards, .. pcs.DiscardPile.Cards];

        CardCmd.Upgrade(allCards.Where(c => !c.IsUpgraded).TakeRandom(Amount, Owner.Player.RunState.Rng.CombatCardSelection), CardPreviewStyle.HorizontalLayout);
    }
}