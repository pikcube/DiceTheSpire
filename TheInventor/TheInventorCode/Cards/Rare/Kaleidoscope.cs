using DiceTheSpireCore.DiceTheSpireCoreCode;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using Pikcube.Common.Keywords;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class Kaleidoscope() : TheInventorCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(Hook);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Inspect, DynamicVars.Cards), HoverTipFactory.FromKeyword(BlinkModel.Blink)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel[] topCards = [.. PileType.Draw.GetPile(Owner).Cards.Take(DynamicVars.Cards.IntValue)];

        if (topCards.Length == 0)
        {
            return;
        }

        CardSelectorPrefs prefs = new(new LocString("card_selection", "TO_BLINK"), 0, topCards.Length);

        CardModel[] selectedCards = [.. await CardSelectCmd.FromSimpleGrid(choiceContext, topCards, Owner, prefs)];

        List<CardTransformation> transforms = [];

        List<CardPoolModel> pools = [.. Owner.UnlockState.CharacterCardPools];
        if (pools.Count > 1)
        {
            pools.Remove(Owner.Character.CardPool);
        }

        IEnumerable<CardModel> results = CardFactory.GetDistinctForCombat(Owner, pools.SelectMany(p => p.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)), selectedCards.Length, Owner.RunState.Rng.CombatCardGeneration);

        foreach ((CardModel o, CardModel r) in selectedCards.Zip(results))
        {
            transforms.Add(new CardTransformation(o, r));
        }

        IEnumerable<CardPileAddResult> trans = await CardCmd.Transform(transforms, null, CardPreviewStyle.None);

        CardModel[] cards = [.. trans.Select(r => r.cardAdded)];
        await BlinkModel.BlinkCardsAsync(choiceContext, cards);

        foreach (IOnInspectListener listener in Owner.RunState.IterateHookListeners(Owner.Creature.CombatState).OfType<IOnInspectListener>())
        {
            await listener.OnInspectAsync(choiceContext, cards.Length, cards, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(2);
    }
}