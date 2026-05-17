using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Cards;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Keywords;

namespace TheInventor.TheInventorCode.Relics;

[UsedImplicitly]
public class Gadget : TheInventorRelic
{
    static Gadget()
    {
        ModHelper.SubscribeForCombatStateHooks("TheInventor.Gadgets", state => state.Players
            .SelectMany(p => p.Relics.OfType<Gadget>())
            .Select(g => g.LinkedGadget));
        ModHelper.SubscribeForRunStateHooks("TheInventor.Gadgets", state => state.Players
            .SelectMany(p => p.Relics.OfType<Gadget>())
            .Select(g => g.LinkedGadget));
    }
    public static Dictionary<string, AbstractGadget> AllGadgets { get; } = [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(nameof(GadgetText))];

    private string GadgetText
    {
        get => ((StringVar)DynamicVars[nameof(GadgetText)]).StringValue; 
        set => ((StringVar)DynamicVars[nameof(GadgetText)]).StringValue = value;
    }
    
    public override RelicRarity Rarity => RelicRarity.Event;

    [SavedProperty] 
    public string GadgetId 
    { 
        get;
        set
        {
            field = value;
            GadgetText = $"{LinkedGadget.GadgetText}";
        }
    } = "default";

    public AbstractGadget LinkedGadget
    {
        get
        {
            if (field?.GadgetId != GadgetId || field.Parent != this)
            {
                field = AllGadgets[GadgetId].GetMutable(this);
            }
            return field;
        }
    }

    public override RelicModel? GetUpgradeReplacement()
    {
        return ModelDb.Relic<Gadget>();
    }

    public override async Task AfterObtained()
    {
        if (Owner.Relics.OfType<Gadget>().Count() > 1)
        {
            //Nope. Nope. Do not. Under no circumstances. There can be only 1.
            await RelicCmd.Remove(this);
            return;
        }

        GadgetId = ModelDb.GetModel<HeatRay>().GadgetId;
    }

    public override async Task AfterCombatVictory(CombatRoom room)
    {
        List<TheInventorCard> cards = [.. Owner.Deck.Cards.OfType<TheInventorCard>().Where(c => c.IsRemovable && c.OnScrap() != nameof(DefaultGadget))];
        List<TheInventorCard> scrapCards = [.. cards.Where(c => c.Keywords.Contains(ScrapKeyword.Scrap))];
        List<TheInventorCard> otherCards = [.. cards.Where(c => !c.Keywords.Contains(ScrapKeyword.Scrap))];

        cards.Clear();
        cards.AddRange(ShuffleForScrap(scrapCards));
        cards.AddRange(ShuffleForScrap(otherCards));

        CardModel[] options = [.. cards.Take(3)];

        if (options.Length < 3)
        {
            List<CardModel> lastResort =
            [
                ..Owner.Deck.Cards.Where(c =>
                    c is not TheInventorCard && 
                    c.Rarity is CardRarity.Basic or CardRarity.Common or CardRarity.Uncommon or CardRarity.Rare or CardRarity.Event && 
                    c.IsRemovable)
            ];

            Owner.PlayerRng.Rewards.Shuffle(lastResort);

            options = [.. options, .. lastResort];
            options = [.. options.Take(3)];

            if (options.Length == 0)
            {
                //Well shit, the player's deck is literally empty except for Eternal cards.
                //Hope they aren't totally boned right now.
                GadgetId = nameof(BrokenGadget);
                return;
            }
        }

        CardModel? choice = await CardSelectCmd.FromChooseACardScreen(new HookPlayerChoiceContext(Owner, Owner.NetId, GameActionType.Any), cards, Owner);

        if (choice is not null)
        {
            await CardPileCmd.RemoveFromDeck(choice, false);
        }

        if (choice is not TheInventorCard scrapCard)
        {
            GadgetId = nameof(BrokenGadget);
            return;
        }

        GadgetId = scrapCard.OnScrap();
    }

    private IEnumerable<TheInventorCard> ShuffleForScrap(List<TheInventorCard> scrapCards)
    {
        Owner.PlayerRng.Rewards.Shuffle(scrapCards);
        return scrapCards.OrderByDescending(c => PlayedThisCombat.ContainsValue(c));
    }

    private Dictionary<CardModel, TheInventorCard> PlayedThisCombat { get; set; } = [];

    protected override void AfterCloned()
    {
        base.AfterCloned();
        PlayedThisCombat = [];
    }

    public override Task BeforeCombatStart()
    {
        PlayedThisCombat.Clear();
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner || PlayedThisCombat.ContainsKey(cardPlay.Card) || cardPlay.Card.DeckVersion is not TheInventorCard deckVersion)
        {
            return Task.CompletedTask;
        }

        PlayedThisCombat[cardPlay.Card] = deckVersion;
        return Task.CompletedTask;
    }
}