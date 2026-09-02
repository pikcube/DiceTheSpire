using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Pikcube.Common.Keywords;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Utilities;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class ThisIsFine() : TheInventorCard(-1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override bool HasTurnEndInHandEffect => true;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(BlinkModel.Blink)];

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        await InventorHelperFunctions.AutoPlayFromDrawPileAndBlink(choiceContext, Owner, IsUpgraded ? 2 : 1, CardPilePosition.Top);

    }

    public override string GetScrapId => nameof(Fury);
}