using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Listeners;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Relics;


public class ElectricalGloves : TheInventorRelic, IOnShockListener
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(CardKeyword.Retain),
        HoverTipFactory.FromKeyword(ShockModel.Shock)
    ];

    public async Task AfterCardShockedAsync(PlayerChoiceContext choiceContext, CardModel card)
    {
        
    }
}