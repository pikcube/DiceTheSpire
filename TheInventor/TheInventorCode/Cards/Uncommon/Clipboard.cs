using DiceTheSpireCore.DiceTheSpireCoreCode;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Keywords;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Uncommon;


public class Clipboard() : TheInventorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
{
    public override string GetScrapId => nameof(Accelerate);

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips =>
        [HoverTipFactory.Static(BetterStaticHoverTips.Inspect, DynamicVars.Cards), HoverTipFactory.FromKeyword(BlinkModel.Blink)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        int cards = DynamicVars.Cards.IntValue;
        Dictionary<Player, Task<IEnumerable<CardModel>>> results = [];

        foreach (Player p in CombatState.Players)
        {
            CardModel[] topCards = [.. PileType.Draw.GetPile(p).Cards.Take(cards)];

            CardSelectorPrefs prefs = new(new LocString("card_selection", "TO_BLINK"), 0, topCards.Length);

            results.Add(p, CardSelectCmd.FromSimpleGrid(new BlockingPlayerChoiceContext(), topCards, p, prefs));
        }

        await Task.WhenAll(results.Values);

        foreach ((Player p, Task<IEnumerable<CardModel>> value) in results)
        {
            CardModel[] r = [.. await value];
            
            await CardPileCmd.Add(r, PileType.Exhaust, skipVisuals: true);
            foreach (CardModel c in r)
            {
                await CardPileCmd.Add(c, PileType.Draw, CardPilePosition.Top, skipVisuals: true);
                await BlinkModel.BlinkCardAsync(choiceContext, c);
            }

            foreach (IOnInspectListener listener in p.RunState.IterateHookListeners(p.Creature.CombatState).OfType<IOnInspectListener>())
            {
                await listener.OnInspectAsync(choiceContext, cards, r, p);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(2);
    }
}