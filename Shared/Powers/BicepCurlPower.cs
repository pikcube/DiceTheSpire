using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Listeners;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Shared.Powers;

public class BicepCurlPower : DiceTheSpirePower, IAfterNudgeListener, IAfterBumpListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Nudge), HoverTipFactory.Static(BetterStaticHoverTips.Bump)];
    public async Task AfterNudgeAsync(CardModel card, int originalCost, int getAmountToSpend, NudgeDuration duration)
    {
        if (Owner.Player?.PlayerCombatState is null)
        {
            return;
        }
        await CreatureCmd.GainBlock(Owner, Amount, BlockProps.nonCardUnpowered, null);
    }
    public async Task AfterBumpAsync(PlayerChoiceContext choiceContext, CardModel card, CardModel? copy)
    {
        if (Owner.Player?.PlayerCombatState is null)
        {
            return;
        }
        await CreatureCmd.GainBlock(Owner, Amount, BlockProps.nonCardUnpowered, null);
    }

}