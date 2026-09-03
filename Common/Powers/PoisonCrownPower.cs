using DiceTheSpire.Common.Listeners;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Common.Powers;

public class PoisonCrownPower : TheThiefPower, IModifyPipOnPlayListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    Player IModifyPipOnPlayListener.Owner => Owner.Player ?? throw new InvalidOperationException();

    public LocString PipDescription
    {
        get
        {
            LocString l = new LocString("powers", Id.Entry + ".pipDescription").WithDynamicVars(DynamicVars); 
            l.Add("Amount", Amount);
            return l;
        }
    }

    public IEnumerable<IHoverTip> PipHoverTips => [HoverTipFactory.FromPower<PoisonPower>()];

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power != this)
        {
            return;
        }

        if (Owner.Player is null)
        {
            await PowerCmd.Remove(this);
        }
    }

    public async Task ModifyPipOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }

        await PowerCmd.Apply<PoisonPower>(choiceContext, CombatState.Enemies, Amount, Owner, cardPlay.Card);
    }
}