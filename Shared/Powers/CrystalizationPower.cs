using DiceTheSpire.Shared.Cards;
using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Newtonsoft.Json.Linq;

namespace DiceTheSpire.Shared.Powers;
public class CrystalizationPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public int CrystalizeRerollsLeftThisTurn
    {
        get;
        set;
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        return player != Owner.Player ? count : count + Amount;
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (fromHandDraw != true)
        {
            return;
        }

        CrystalizeRerollsLeftThisTurn = Amount;

        for (int n = 0; n < CrystalizeRerollsLeftThisTurn; ++n)
        {
            await RerollCmd.RerollAsync(choiceContext, card, RerollDuration.UntilEndOfTurnOrPlayed);
        }
    }

    //public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    //{
    //    CrystalizeRerollsLeftThisTurn = Amount;
    //}
}