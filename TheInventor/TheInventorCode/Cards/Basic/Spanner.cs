using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;
using TheInventor.TheInventorCode.Cards.Ancient;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Basic;

public class Spanner() : TheInventorCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self), ITranscendenceCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(BlinkModel.Blink)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await base.OnPlay(choiceContext, cardPlay);
        CardSelectorPrefs cardSelectorPrefs = new(new LocString("card_selection", "TO_BLINK"), 2, 2);
        IEnumerable<CardModel> results = await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this);
        await Task.WhenAll(results.Select(card => card.BlinkAsync(choiceContext)));
        IEnumerable<CardModel> toRetrive = PileType.Discard.GetPile(Owner).Cards
            .TakeRandom(DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardSelection);
        await CardPileCmd.Add(toRetrive, PileType.Hand, CardPilePosition.Bottom, this);
    }

    public override string GetScrapId => nameof(BattleWrench);

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    public CardModel GetTranscendenceTransformedCard() => SteelWrench.CreateWithoutOwner();
}