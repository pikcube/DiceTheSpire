using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Listeners;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Shared.Powers;

public class AcornShieldPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Nudge), HoverTipFactory.Static(BetterStaticHoverTips.Bump)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Player?.PlayerCombatState is null)
        {
            return;
        }

        if (cardPlay.Card.EnergyCost.Canonical != cardPlay.Resources.EnergySpent)
        {
            await CreatureCmd.GainBlock(Owner, Amount, BlockProps.nonCardUnpowered, null);
        }
    }

}