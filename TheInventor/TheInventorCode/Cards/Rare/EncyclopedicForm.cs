using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Keywords;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Rare;


public class EncyclopedicForm() : TheInventorCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{ 
    public override string GetScrapId => nameof(BurstOfKnowledge);

    public override IEnumerable<CardKeyword> CanonicalKeywords => [ScrapKeyword.Scrap];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<TemporaryGadgetPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(ScrapKeyword.Scrap);
    }
}