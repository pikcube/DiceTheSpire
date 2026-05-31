using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Rare;


public class Befuddle() : TheInventorCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(Hook);

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel[] discard = [.. PileType.Discard.GetPile(Owner).Cards];
        CardModel[] draw = [.. PileType.Draw.GetPile(Owner).Cards];

        Task<IReadOnlyList<CardPileAddResult>> discardMove = CardPileCmd.Add(discard, PileType.Draw);
        Task<IReadOnlyList<CardPileAddResult>> drawMove = CardPileCmd.Add(draw, PileType.Discard);

        await Task.WhenAll(discardMove, drawMove);
    }
}