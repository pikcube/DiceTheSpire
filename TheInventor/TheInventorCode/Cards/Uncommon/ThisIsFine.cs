using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Uncommon;

public class ThisIsFine() : TheInventorCard(-1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool HasTurnEndInHandEffect => true;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        await CardPileCmd.AutoPlayFromDrawPile(choiceContext, Owner, IsUpgraded ? 2 : 1, CardPilePosition.Top, false);
    }

    public override string GetScrapId => nameof(MagicDice);
}