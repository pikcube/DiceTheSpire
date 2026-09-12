using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace DiceTheSpire.Inventor.Rare;

public class ThisIsFine() : TheInventorCard(-1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override bool HasTurnEndInHandEffect => true;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(ShockModel.Shock)];

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        await InventorHelperFunctions.AutoPlayFromDrawPileAndShock(choiceContext, Owner, IsUpgraded ? 2 : 1, CardPilePosition.Top);

    }

    public override string GetScrapId => nameof(Fury);
}