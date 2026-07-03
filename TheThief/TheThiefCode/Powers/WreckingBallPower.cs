using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheThief.TheThiefCode.Powers;

public class WreckingBallPower : TheThiefPower, IModifyPipOnPlayListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    Player IModifyPipOnPlayListener.Owner => Owner.Player ?? throw new InvalidOperationException();
    public LocString PipDescription {
        get
        {
            LocString l = new LocString("powers", Id.Entry + ".pipDescription").WithDynamicVars(DynamicVars);
            l.Add("Amount", Amount);
            return l;
        }
    }

    public IEnumerable<IHoverTip> PipHoverTips => [];

    public async Task ModifyPipOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.CombatState is null || cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }
        await CreatureCmd.Damage(choiceContext, CombatState.Enemies, Amount, ValueProp.Unpowered, Owner, cardPlay.Card, cardPlay);
    }
}