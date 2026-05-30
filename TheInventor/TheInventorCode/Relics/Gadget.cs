using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;
using TheInventor.TheInventorCode.Cards;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Interfaces;
using TheInventor.TheInventorCode.Keywords;

namespace TheInventor.TheInventorCode.Relics;

[UsedImplicitly]
public class Gadget : TheInventorRelic, IGadgetParent, IRunInitializedListener
{
    static Gadget()
    {
        ModHelper.SubscribeForCombatStateHooks("TheInventor.Gadgets", state => state.Players
            .SelectMany(p => p.Relics.OfType<Gadget>())
            .Select(g => g.LinkedGadgetModel));
        ModHelper.SubscribeForRunStateHooks("TheInventor.Gadgets", state => state.Players
            .SelectMany(p => p.Relics.OfType<Gadget>())
            .Select(g => g.LinkedGadgetModel));
    }
    public static Dictionary<string, GadgetModel> AllGadgets { get; } = [];

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
            GadgetText = $"{LinkedGadgetModel.GadgetText}";
        }
    } = nameof(DefaultGadget);

    public AbstractModel AsModel() => this;

    public GadgetModel LinkedGadgetModel
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

    public override RelicModel GetUpgradeReplacement()
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
        List<CardModel> cards = [.. Owner.Deck.Cards.Where(c => c is 
        {
            IsRemovable: true,
            Rarity: CardRarity.Basic or CardRarity.Common or CardRarity.Uncommon or CardRarity.Rare or CardRarity.Ancient or CardRarity.Event or CardRarity.Curse
        })];

        List<CardModel> scrapCards = [.. cards.Where(c => c.Keywords.Contains(ScrapKeyword.Scrap))];
        List<CardModel> otherCards = [.. cards.Where(c => !c.Keywords.Contains(ScrapKeyword.Scrap))];

        cards.Clear();
        cards.AddRange(ShuffleForScrap(scrapCards));
        cards.AddRange(ShuffleForScrap(otherCards));

        CardModel[] options = [.. cards.Take(3)];

        if (options.Length == 0)
        {
            //Well shit, the player's deck is literally empty except for Eternal cards.
            //Hope they aren't totally boned right now.
            GadgetId = nameof(BrokenGadget);
            return;
        }

        BetterHooks.ModifyCardSelectionScreenTitle += BetterHooksOnModifyCardSelectionScreenTitle;

        CardModel? choice = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), options, Owner);

        BetterHooks.ModifyCardSelectionScreenTitle -= BetterHooksOnModifyCardSelectionScreenTitle;

        if (choice is not null)
        {
            await CardPileCmd.RemoveFromDeck(choice, false);
        }

        if (choice is TheInventorCard scrapCard)
        {
            if (!scrapCard.ModifyScrap(this, LinkedGadgetModel))
            {
                GadgetId = scrapCard.GetScrapId;
                await scrapCard.OnScrapAsync(LinkedGadgetModel);
            }
        }
        else
        {
            GadgetId = GetDefaultGadget(choice);
        }

        foreach (TheInventorCard c in options.OfType<TheInventorCard>().Where(c => c != choice))
        {
            await c.OnSkippedAsync();
        }
    }

    private void BetterHooksOnModifyCardSelectionScreenTitle(NChooseACardSelectionScreen sender, ModifyCardSelectionScreenTitleArgs e)
    {
        e.NewText = "Scrap a Card";
    }

    public static string GetDefaultGadget(CardModel? choice)
    {
        if (choice is null)
        {
            return nameof(BrokenGadget);
        }

        if (choice.Type is CardType.Curse or CardType.Status or CardType.Quest)
        {
            return nameof(CursedGadget);
        }
        
        DynamicVar? bestVar = choice.DynamicVars.Values.OrderBy(var => var switch //Lower value means higher priority var
        {
            PowerVar<VulnerablePower> => 10,
            PowerVar<WeakPower> => 20,
            EnergyVar => 30,
            CardsVar => 40,
            DamageVar or CalculatedDamageVar => 50,
            BlockVar or CalculatedBlockVar => 60,
            _ => 100,
        }).FirstOrDefault();

        return bestVar switch
        {
            PowerVar<VulnerablePower> => nameof(ShortCircuit),
            PowerVar<WeakPower> => nameof(Burrower),
            EnergyVar => nameof(MagicDice),
            CardsVar => nameof(BattleWrench),
            DamageVar or CalculatedDamageVar => choice.TargetType == TargetType.AllEnemies ? nameof(Crack) : nameof(Bonk),
            BlockVar or CalculatedBlockVar => choice.Rarity is CardRarity.Basic or CardRarity.Common ? nameof(Shield) : nameof(WallOfIce),
            _ => choice.Type switch
            {
                CardType.Attack => nameof(Bonk),
                CardType.Skill => nameof(Shield),
                CardType.Power => nameof(BattleWrench),
                CardType.Status or CardType.Curse or CardType.Quest => nameof(CursedGadget),
                _ => nameof(BrokenGadget)
            }
        };
    }

    private IEnumerable<CardModel> ShuffleForScrap(List<CardModel> scrapCards)
    {
        Owner.PlayerRng.Rewards.Shuffle(scrapCards);
        return scrapCards.OrderByDescending(c => PlayedThisCombat.ContainsValue(c));
    }

    private Dictionary<CardModel, CardModel> PlayedThisCombat { get; set; } = [];

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
        if (cardPlay.Card.Owner != Owner || PlayedThisCombat.ContainsKey(cardPlay.Card) || cardPlay.Card.DeckVersion is not CardModel deckVersion)
        {
            return Task.CompletedTask;
        }

        PlayedThisCombat[cardPlay.Card] = deckVersion;
        return Task.CompletedTask;
    }

    public override Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards)
        {
            if (card.Owner != Owner || PlayedThisCombat.ContainsKey(card) ||
                card.DeckVersion is not { } deckVersion)
            {
                continue;
            }

            PlayedThisCombat[card] = deckVersion;
        }

        return Task.CompletedTask;
    }

    public static async Task RechargeAsync(PlayerChoiceContext context, Player owner)
    {
        foreach (IGadgetParent parent in owner.RunState.IterateHookListeners(owner.Creature.CombatState)
                     .OfType<IGadgetParent>().Where(gp => gp.Owner == owner))
        {
            await parent.LinkedGadgetModel.OnRechargeAsync(context, owner);
        }
    }

    public static string GetRandomCombatGadgetId(Rng rng)
    {
        return AllGadgets.Where(g => g.Value.IsAllowedAsTempGadget).TakeRandom(1, rng).Single().Key;
    }

    public void AfterRunInitialized(RunState runState)
    {
        BetterHooks.ModifyCardSelectionScreenTitle -= BetterHooksOnModifyCardSelectionScreenTitle;
    }
}