using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Inventor.Powers;

public class GrindstonePower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Bump)];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner != player.Creature)
        {
            return;
        }

        Flash();

        LocString locString = DiceySelection.ToBump;
        CardSelectorPrefs cardSelectorPrefs = new(locString, Amount);
        IEnumerable<CardModel> result = await CardSelectCmd.FromHand(choiceContext, player, cardSelectorPrefs, null, this);

        foreach (CardModel card in result)
        {
            await card.BumpAsync(choiceContext);
        }
    }
}