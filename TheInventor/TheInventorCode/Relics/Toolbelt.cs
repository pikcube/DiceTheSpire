using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;

namespace TheInventor.TheInventorCode.Relics;

public class Toolbelt : TheInventorRelic, IModifyUnplayableBehaviorListener
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(CardKeyword.Unplayable), HoverTipFactory.FromKeyword(BlinkModel.Blink)
    ];

    public bool ModifyUnplayableBehavior(CardModel card, ref Func<PlayerChoiceContext, CardPlay, Task>? newOnPlay)
    {
        if (card.Owner != Owner)
        {
            return false;
        }

        newOnPlay = NewOnPlayAsync;
        return true;

    }

    private async Task NewOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await cardPlay.Card.BlinkAsync(choiceContext);
        Flash();
    }
}