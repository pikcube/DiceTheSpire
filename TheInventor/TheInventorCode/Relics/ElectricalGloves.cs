using DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace TheInventor.TheInventorCode.Relics;


public class ElectricalGloves : TheInventorRelic, IAfterCardShockedListener
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(CardKeyword.Retain), HoverTipFactory.FromPower<ShockPower>(1)
    ];


    public Task AfterCardShockedAsync(PlayerChoiceContext choiceContext, ShockPower shock, CardModel card)
    {
        if (card.Owner != Owner || card.Keywords.Contains(CardKeyword.Retain) || !card.Keywords.Contains(CardKeyword.Unplayable))
        {
            return Task.CompletedTask;
        }

        card.AddTempKeyword(CardKeyword.Retain, shock);
        card.KeywordsChanged += CardOnKeywordsChanged;

        return Task.CompletedTask;

        void CardOnKeywordsChanged()
        {
            if (card.Keywords.Contains(CardKeyword.Unplayable))
            {
                return;
            }
            card.KeywordsChanged -= CardOnKeywordsChanged;
            card.RemoveTempKeywordEarly(CardKeyword.Retain);
            card.RemoveKeyword(CardKeyword.Retain);
        }
    }
}