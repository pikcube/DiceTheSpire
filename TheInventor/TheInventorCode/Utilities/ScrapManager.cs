using BaseLib.Abstracts;
using BaseLib.Utils;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;
using TheInventor.TheInventorCode.Cards;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Interfaces;
using TheInventor.TheInventorCode.Keywords;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Utilities;

[UsedImplicitly]
public class ScrapManager() : CustomSingletonModel(HookType.Run), IRunInitializedListener, ICreatingNewRunListener
{
    static ScrapManager()
    {
        ModHelper.SubscribeForRunStateHooks(MainFile.ModId, GetRunStateHooks);
    }

    private static IEnumerable<GadgetModel> GetRunStateHooks(RunState runState)
    {
        foreach ((Player p, string? gadgetId) in runState.Players.Where(p => p.Character is Character.TheInventor).Select(p => (p, GadgetId.Get(p))))
        {
            if (gadgetId is null or nameof(Dig))
            {
                continue;
            }

            GadgetModel gadget = AllGadgets[gadgetId];
            if (gadget.HookType != HookType.Run)
            {
                continue;
            }

            TempParent parent = new(p, gadget);
            GadgetModel mutableGadgetModel = parent.LinkedGadgetModel;
            yield return mutableGadgetModel;
        }
    }

    public static readonly SavedSpireField<Player, string> GadgetId = new(() => nameof(DefaultGadget), $"{MainFile.ModId}_{nameof(GadgetId)}");

    public static Dictionary<string, GadgetModel> AllGadgets { get; } = [];

    public override async Task BeforeCombatStart()
    {
        RunState? state = RunManager.Instance.GetPrivateProperty<RunManager, RunState>("State");
        if (state is null)
        {
            return;
        }

        List<Task> tasks = [];

        foreach (Player player in state.Players)
        {
            if (player.Character is not Character.TheInventor)
            {
                return;
            }

            string id = GadgetId.Get(player) ?? nameof(BrokenGadget);
            if (AllGadgets[id].HookType != HookType.Combat)
            {
                id = nameof(BrokenGadget);
            }

            GadgetPower p = (GadgetPower)ModelDb.Power<GadgetPower>().ToMutable();
            p.InitialGadgetId = id;

            await PowerCmd.Apply(new BlockingPlayerChoiceContext(), p, player.Creature, 1, player.Creature, null);
            tasks.Add(p.AfterRandomizedAsync());
        }

        await Task.WhenAll(tasks);
    }

    private static List<Player> Ignore { get; set; } = [];

    public static bool IgnoreNext(Player player)
    {
        if (!Ignore.Contains(player))
        {
            return false;
        }

        Ignore.Remove(player);
        return true;

    }

    public static async Task DoScrapAsyncFor(RewardsSet rewardsSet)
    {
        Player p = rewardsSet.Player;
        List<CardModel> cards = [.. p.Deck.Cards.Where(c => c is
        {
            IsRemovable: true,
            Rarity: CardRarity.Basic or CardRarity.Common or CardRarity.Uncommon or CardRarity.Rare or CardRarity.Ancient or CardRarity.Event or CardRarity.Curse
        })];

        List<CardModel> scrapCards = [.. cards.Where(c => c.Keywords.Contains(ScrapKeyword.Scrap))];
        List<CardModel> otherCards = [.. cards.Where(c => !c.Keywords.Contains(ScrapKeyword.Scrap))];

        cards.Clear();
        cards.AddRange(ShuffleForScrap(p, scrapCards));
        cards.AddRange(ShuffleForScrap(p, otherCards));

        CardModel[] cardModels = [.. cards.Take(3)];
        if (cardModels.Length == 0)
        {
            //Well shit, the player's deck is literally empty except for Eternal cards.
            //Hope they aren't totally boned right now.
            GadgetId.Set(p, nameof(BrokenGadget));
        }

        CardModel[] choiceClones = [..cardModels.Select(c => (CardModel)c.ClonePreservingMutability())];

        if (LocalContext.IsMe(p))
        {
            BetterHooks.ModifyCardSelectionScreenTitle += BetterHooksOnModifyCardSelectionScreenTitle;
            TheInventorCard.EnableTipsOnCards.AddRange(choiceClones);
        }

        CardModel? clone = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), choiceClones, p);
        CardModel? choice = cardModels.ElementAtOrDefault(choiceClones.IndexOf(clone));
        
        if (LocalContext.IsMe(p))
        {
            foreach (CardModel c in choiceClones)
            {
                TheInventorCard.EnableTipsOnCards.Remove(c);
            }
            BetterHooks.ModifyCardSelectionScreenTitle -= BetterHooksOnModifyCardSelectionScreenTitle;
        }

        if (choice is not null)
        {
            await CardPileCmd.RemoveFromDeck(choice, false);
        }

        if (choice is TheInventorCard scrapCard)
        {
            if (!scrapCard.ModifyScrap())
            {
                GadgetId.Set(p, scrapCard.GetScrapId);
                await scrapCard.OnScrapAsync();
            }
        }
        else
        {
            GadgetId.Set(p, GetDefaultGadget(choice));
        }

        foreach (TheInventorCard c in p.Deck.Cards.OfType<TheInventorCard>().Where(c => c != choice))
        {
            await c.OnSkippedAsync();
        }

        Ignore.Add(rewardsSet.Player);
        
        await rewardsSet.Offer();
    }

    private static void BetterHooksOnModifyCardSelectionScreenTitle(NChooseACardSelectionScreen sender, ModifyCardSelectionScreenTitleArgs e)
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

        if (choice.Rarity == CardRarity.Ancient)
        {
            return nameof(BattleWrench);
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
            DamageVar or CalculatedDamageVar => choice.TargetType == TargetType.AllEnemies ? nameof(Blowtorch) : nameof(Bonk),
            BlockVar or CalculatedBlockVar => choice.Rarity is CardRarity.Basic or CardRarity.Common ? nameof(Shield) : nameof(WallOfIce),
            _ => choice.Type switch
            {
                CardType.Attack => choice.TargetType is TargetType.AnyEnemy ? nameof(Bonk) : nameof(Blowtorch),
                CardType.Skill => choice.Rarity is CardRarity.Common or CardRarity.Basic ? nameof(Shield) : nameof(WallOfIce),
                CardType.Power => choice.Rarity is CardRarity.Uncommon ? nameof(MagicSpanner) : nameof(BattleWrench),
                CardType.Status or CardType.Curse or CardType.Quest => nameof(CursedGadget),
                _ => nameof(BrokenGadget)
            }
        };
    }

    private static List<CardModel> ShuffleForScrap(Player p, List<CardModel> scrapCards)
    {
        p.PlayerRng.Rewards.Shuffle(scrapCards);
        return scrapCards;
    }

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

    public static List<IGadgetParent> GetGadgetParents(Player owner) => [.. owner.RunState
        .IterateHookListeners(owner.Creature.CombatState)
        .OfType<IGadgetParent>()
        .Where(p => p.Owner == owner)];

    public void AfterCreatingNewRun(RunState runState, IReadOnlyList<Player> players, IReadOnlyList<ActModel> acts, IReadOnlyList<ModifierModel> modifiers,
        GameMode gameMode, int ascensionLevel, string seed)
    {
        foreach (Player p in players.Where(p => p.Character is Character.TheInventor))
        {
            GadgetId.Set(p, ascensionLevel > 5 ? nameof(Efficiency) : nameof(HeatRay));
        }
    }
}

internal class TempParent : IGadgetParent
{
    public TempParent(Player player, GadgetModel gadget)
    {
        Owner = player;
        LinkedGadgetModel = gadget.GetMutable(this);
    }

    public string GadgetId
    {
        set {}
    }
    public Player Owner { get; }
    public GadgetModel LinkedGadgetModel { get; internal set; }

    public void Flash()
    {
    }

    public Task AfterRandomizedAsync()
    {
        return Task.CompletedTask;
    }

    public void Update()
    {
    }
}