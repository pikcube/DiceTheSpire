using DiceTheSpire.Common.Listeners;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;

namespace DiceTheSpire.Common.Relics;

public class Toolbelt : TheInventorRelic, IModifyUnplayableBehaviorListener
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(CardKeyword.Unplayable), HoverTipFactory.FromKeyword(BlinkModel.Blink)
    ];

    public bool ModifyUnplayableBehavior(CardModel card)
    {
        return card.Owner == Owner && card.Keywords.Contains(CardKeyword.Unplayable);
    }

    public bool TryModifyTargetType(CardModel card, ref TargetType result)
    {
        if (!ModifyUnplayableBehavior(card))
        {
            return false;
        }

        result = TargetType.Self;
        return true;

    }

    public bool TryModifyOnPlay(CardModel card, ref Func<PlayerChoiceContext, CardPlay, Task> task)
    {
        if (!ModifyUnplayableBehavior(card))
        {
            return false;
        }

        task = NewOnPlayAsync;

        return true;
    }

    private async Task NewOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await cardPlay.Card.BlinkAsync(choiceContext);
        Flash();
    }
}