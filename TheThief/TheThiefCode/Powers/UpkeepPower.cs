using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheThief.TheThiefCode.Powers;

public class UpkeepPower : TheThiefPower, IModifyPipOnPlayListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    Player IModifyPipOnPlayListener.Owner => Owner.Player ?? throw new InvalidOperationException();
    public IEnumerable<IHoverTip> PipHoverTips => [HoverTipFactory.FromPower<PoisonPower>(), HoverTipFactory.ForEnergy(this)];

    public LocString PipDescription
    {
        get
        {
            LocString l = new LocString("powers", Id.Entry + ".pipDescription").WithDynamicVars(DynamicVars);
            l.Add("Amount", Amount);
            return l;
        }
    }
    public async Task ModifyPipOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return;
        }
        await PlayerCmd.GainEnergy(1, Owner.Player);
    }
}