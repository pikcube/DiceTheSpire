using DiceTheSpire.Shared.Cards;
using DiceTheSpire.Shared.Listeners;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Powers;

public class CampfirePower : TheThiefPower, IModifyPipOnPlayListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public bool ShouldModify(Pip pip) => pip.Owner == Owner.Player;

    public LocString PipDescription
    {
        get
        {
            LocString l = new LocString("powers", Id.Entry + ".pipDescription").WithDynamicVars(DynamicVars);
            l.Add("Amount", Amount);
            return l;
        }
    }

    public IEnumerable<IHoverTip> PipHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

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

    public async Task ModifyPipOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay, Pip pip)
    {
        if (!ShouldModify(pip))
        {
            return;
        }

        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, Amount, Owner, cardPlay.Card);
    }
}