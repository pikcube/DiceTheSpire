using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
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
using TheInventor.TheInventorCode.Cards.Token;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Interfaces;
using TheInventor.TheInventorCode.Keywords;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Relics;

[UsedImplicitly]
public class Gadget : TheInventorRelic, IGadgetParent, IRunInitializedListener
{
    static Gadget()
    {
        ModHelper.SubscribeForCombatStateHooks("TheInventor.Gadgets", GetCombatHooks);
        ModHelper.SubscribeForRunStateHooks("TheInventor.Gadgets", GetRunHooks);
    }

    private static AbstractModel[] GetRunHooks(IRunState state)
    {
        return [.. state.Players.SelectMany(p => p.Relics.OfType<Gadget>())
            .Select(g => g.LinkedGadgetModel)
            .Where(g => g.HookType == CustomSingletonModel.HookType.Run)];
    }

    private static AbstractModel[] GetCombatHooks(ICombatState state)
    {
        return [.. state.Players.SelectMany(p => p.Relics.OfType<Gadget>())
            .Select(g => g.LinkedGadgetModel)
            .Where(g => g.HookType == CustomSingletonModel.HookType.Combat)];
    }

    public static Dictionary<string, GadgetModel> AllGadgets { get; } = [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(nameof(GadgetText))];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(InventorStaticHoverTips.Scrap)];

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
            GadgetText = LinkedGadgetModel.GadgetText;
        }
    } = nameof(DefaultGadget);

    [SavedProperty]
    public int KindnessLeft
    {
        get;
        set;
    }

    public AbstractModel AsModel() => this;
    public async Task AfterRandomizedAsync()
    {
        Flash([Owner.Creature]);
        await GadgetCard.ShowAsync(LinkedGadgetModel);
    }

    public void Update()
    {
        GadgetText = $"{LinkedGadgetModel.GadgetText}";
    }

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
        if (InventorDebugConfig.IsKind)
        {
            KindnessLeft = 4;
        }
        else
        {
            KindnessLeft = 0;
        }
    }

    public override async Task AfterCombatVictory(CombatRoom room)
    {
        if (KindnessLeft > 0)
        {
            --KindnessLeft;
        }
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

        foreach (TheInventorCard c in Owner.Deck.Cards.OfType<TheInventorCard>().Where(c => c != choice))
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
            PowerVar<PoisonPower> => 10,
            PowerVar<ThornsPower> => 15,
            PowerVar<VigorPower> => 20,
            PowerVar<VulnerablePower> => 30,
            PowerVar<WeakPower> => 40,
            EnergyVar => 50,
            CardsVar => 60,
            DamageVar or CalculatedDamageVar => 70,
            BlockVar or CalculatedBlockVar => 80,
            _ => 100,
        }).FirstOrDefault();

        return bestVar switch
        {
            PowerVar<PoisonPower> => nameof(PoisonArrow),
            PowerVar<VigorPower> => nameof(PowerUp),
            PowerVar<ThornsPower> => nameof(Needle),
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
        if (KindnessLeft > 0)
        {
            return scrapCards.OrderByDescending(c => c.Rarity == CardRarity.Basic);
        }
        //return scrapCards.OrderByDescending(c => PlayedThisCombat.ContainsValue(c));
        return scrapCards;
    }

    //private Dictionary<CardModel, CardModel> PlayedThisCombat { get; set; } = [];
    /*
    protected override void AfterCloned()
    {
        base.AfterCloned();
        PlayedThisCombat = [];
    }

    public override async Task BeforeCombatStart()
    {
        PlayedThisCombat.Clear();

        Flash([Owner.Creature]);
        
        await GadgetCard.ShowAsync(LinkedGadgetModel);
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner || PlayedThisCombat.ContainsKey(cardPlay.Card) || cardPlay.Card.DeckVersion is null)
        {
            return Task.CompletedTask;
        }

        PlayedThisCombat[cardPlay.Card] = cardPlay.Card.DeckVersion;
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
    */

    public static string GetRandomCombatGadgetId(Rng rng)
    {
        return AllGadgets.Where(g => g.Value.IsAllowedAsTempGadget).TakeRandom(1, rng).Single().Key;
    }

    public void AfterRunInitialized(RunState runState)
    {
        BetterHooks.ModifyCardSelectionScreenTitle -= BetterHooksOnModifyCardSelectionScreenTitle;
    }

    public static async Task RandomizeAllGadgetsAsync(PlayerChoiceContext choiceContext, Player owner, CardModel? cardSource)
    {
        List<IGadgetParent> gadgetParents = GetGadgetParents(owner);

        if (gadgetParents.Count == 0)
        {
            TemporaryGadgetPower? temp = await TemporaryGadgetPower.ApplyAsync(choiceContext, owner.Creature, 1, owner.Creature, cardSource);
            if (temp is not null)
            {
                gadgetParents.Add(temp);
            }
        }

        foreach (IGadgetParent parent in gadgetParents)
        {
            parent.GadgetId = GetRandomCombatGadgetId(owner.RunState.Rng.CombatOrbGeneration);
            await parent.AfterRandomizedAsync();
        }
    }

    public static List<IGadgetParent> GetGadgetParents(Player owner) => [.. owner.RunState.IterateHookListeners(owner.Creature.CombatState).OfType<IGadgetParent>()];
}