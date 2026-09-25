using BaseLib.Abstracts;
using BaseLib.Utils;
using DiceTheSpire.Inventor;
using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Patches;
using DiceTheSpire.Shared.Powers;
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
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;

namespace DiceTheSpire.Shared.Utility;

[UsedImplicitly]
public class ScrapManager() : CustomSingletonModel(HookType.Run), IRunInitializedListener, ICreatingNewRunListener
{
    public enum ScrapDebugMode
    {
        Default = 0,
        Optional = 1,
        Off = 2,
    }

    public static ScrapDebugMode DebugMode { get; set; } = ScrapDebugMode.Default;

    static ScrapManager()
    {
        ModHelper.SubscribeForRunStateHooks(MainFile.ModId, GetRunStateHooks);
    }

    private static IEnumerable<GadgetModel> GetRunStateHooks(RunState runState)
    {
        foreach ((Player p, string? gadgetId) in runState.Players.Where(p => p.Character is TheInventor).Select(p => (p, GadgetId(p))))
        {
            if (gadgetId is null)
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

    public static string GadgetId(Player p) => CurrentGadgetId.Get(p) ?? nameof(DefaultGadget);
    public static int GadgetOrigin(Player p) => LastScrapLocation.Get(p);

    public static void SetGadgetInfo(Player p, string gadgetId, bool isFromScrapScreen)
    {
        CurrentGadgetId.Set(p, gadgetId);
        if (isFromScrapScreen)
        {
            LastScrapLocation.Set(p, p.RunState.TotalFloor);
        }
    }

    private static readonly SavedSpireField<Player, string> CurrentGadgetId = new(() => nameof(DefaultGadget), $"{MainFile.ModId}_{nameof(GadgetId)}");

    private static readonly SavedSpireField<Player, int> LastScrapLocation =
        new(() => -1, $"{MainFile.ModId}_{nameof(LastScrapLocation)}");

    public static Dictionary<string, GadgetModel> AllGadgets { get; } = [];

    public override async Task BeforeCombatStart()
    {
        RunState? state = RunManager.Instance.PrivatePropertyWrapper<RunManager, RunState>("State").Value;
        if (state is null)
        {
            return;
        }

        List<Task> tasks = [];

        foreach (Player player in state.Players)
        {
            if (player.Character is not TheInventor)
            {
                return;
            }

            string id = GadgetId(player);
            if (AllGadgets[id].HookType != HookType.Combat)
            {
                id = nameof(BrokenGadget);
            }

            GadgetPower p = (GadgetPower)ModelDb.Power<GadgetPower>().ToMutable();
            p.InitialGadgetId = id;

            await PowerCmd.Apply(new BlockingPlayerChoiceContext(), p, player.Creature, 1, player.Creature, null);
            tasks.Add(p.UpdateAndPreviewAsync());
        }

        await Task.WhenAll(tasks);
    }

    public static async Task DoScrapForThenOfferAsync(RewardsSet rewardsSet)
    {
        ArgumentNullException.ThrowIfNull(NMapScreen.Instance);

        bool canTravel = NMapScreen.Instance.IsTravelEnabled;

        if (LocalContext.IsMe(rewardsSet.Player))
        {
            NMapScreen.Instance.SetTravelEnabled(false);
        }

        try
        {
            Player p = rewardsSet.Player;
            CardModel? choice = await SelectCardForScrapAsync(p);

            if (choice is not null)
            {
                await CardPileCmd.RemoveFromDeck(choice);
            }

            await CreateGadgetAsync(rewardsSet.Rewards, choice, p);
        }
        finally
        {
            if (LocalContext.IsMe(rewardsSet.Player))
            {
                NMapScreen.Instance.SetTravelEnabled(canTravel);
            }
        }

        await RewardSetOfferPatches.OfferRewardAfterScrapAsync(rewardsSet);
    }

    public static async Task DoScrapForThenResumeEvent(Player p)
    {
        ArgumentNullException.ThrowIfNull(NMapScreen.Instance);

        bool canTravel = NMapScreen.Instance.IsTravelEnabled;

        if (LocalContext.IsMe(p))
        {
            NMapScreen.Instance.SetTravelEnabled(false);
        }

        try
        {
            CardModel? choice = await SelectCardForScrapAsync(p);

            if (choice is not null)
            {
                await CardPileCmd.RemoveFromDeck(choice);
            }

            await CreateGadgetAsync(null, choice, p);
        }
        finally
        {
            if (LocalContext.IsMe(p))
            {
                NMapScreen.Instance.SetTravelEnabled(canTravel);
            }
        }
    }

    private static async Task CreateGadgetAsync(List<Reward>? rewards, CardModel? choice, Player p)
    {
        if (choice is TheInventorCard scrapCard)
        {
            if (scrapCard.ModifyScrap())
            {
                return;
            }

            SetGadgetInfo(p, scrapCard.GetScrapId, true);
            await scrapCard.OnScrapAsync();
        }
        else
        {
            string gadgetId = GetDefaultGadget(choice);
            SetGadgetInfo(p, gadgetId, true);
        }

        string newScrapId = GadgetId(p);

        TempParent parent = new(p, AllGadgets[newScrapId]);
        await parent.LinkedGadgetModel.OnPickupAsync();
        
        if (!parent.LinkedGadgetModel.ModifiesRewards)
        {
            return;
        }

        if (rewards is null)
        {
            await RewardsCmd.OfferCustom(p, []);
        }
        else
        {
            parent.LinkedGadgetModel.TryModifyRewards(p, rewards, p.RunState.CurrentRoom);
        }

        
    }

    private static async Task<CardModel?> SelectCardForScrapAsync(Player p)
    {
        List<CardModel> cards = [.. p.Deck.Cards.Where(CanScrapCard)];

        CreateScrapLists(cards, p, out List<CardModel> scrapCards, out List<CardModel> otherCards);

        DiceyHooks.OnModifyScrapPriority(p.RunState, p, ref scrapCards, ref otherCards);

        cards.Clear();
        cards.AddRange(scrapCards);
        cards.AddRange(otherCards);

        CardModel[] cardModels = [.. cards.Take(3)];
        if (cardModels.Length == 0)
        {
            //Well shit, the player's deck is literally empty except for Eternal cards.
            //Hope they aren't totally boned right now.
            return null;
        }

        CardModel[] choiceClones = [.. cardModels.Select(c => (CardModel)c.ClonePreservingMutability())];

        if (LocalContext.IsMe(p))
        {
            BetterHooks.ModifyCardSelectionScreenTitle += BetterHooksOnModifyCardSelectionScreenTitle;
            TheInventorCard.EnableTipsOnCards.AddRange(choiceClones);
        }

        CardModel? clone = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), choiceClones, p, DebugMode == ScrapDebugMode.Optional);
        CardModel? choice = cardModels.ElementAtOrDefault(choiceClones.IndexOf(clone));

        if (!LocalContext.IsMe(p))
        {
            return choice;
        }

        foreach (CardModel c in choiceClones)
        {
            TheInventorCard.EnableTipsOnCards.Remove(c);
        }
        BetterHooks.ModifyCardSelectionScreenTitle -= BetterHooksOnModifyCardSelectionScreenTitle;

        return choice;
    }

    public static bool CanScrapCard(CardModel card)
    {
        if (IsAlwaysOfferedAsScrap(card))
        {
            return true;
        }

        return card is
        {
            IsRemovable: true,
            Rarity:
            CardRarity.Basic or
            CardRarity.Common or
            CardRarity.Uncommon or
            CardRarity.Rare or
            CardRarity.Ancient or
            CardRarity.Event or
            CardRarity.Curse
        };
    }

    private static void CreateScrapLists(List<CardModel> cards, Player p, out List<CardModel> scrapCards, out List<CardModel> otherCards)
    {
        scrapCards = [];
        otherCards = [];
        foreach (IGrouping<bool, CardModel> group in cards.GroupBy(IsAlwaysOfferedAsScrap))
        {
            if (group.Key)
            {
                scrapCards.AddRange(group);
            }
            else
            {
                otherCards.AddRange(group);
            }
        }
        p.PlayerRng.Rewards.Shuffle(scrapCards);
        p.PlayerRng.Rewards.Shuffle(otherCards);
    }

    public static bool IsAlwaysOfferedAsScrap(CardModel c)
    {
        return c is IScrapCard { IsAlwaysOfferedAsScrap: true } || c.Enchantment is IScrapCard { IsAlwaysOfferedAsScrap: true };
    }

    private static void BetterHooksOnModifyCardSelectionScreenTitle(NChooseACardSelectionScreen sender, ModifyCardSelectionScreenTitleArgs e)
    {
        e.NewText = "Scrap a Card";
    }

    public static string GetDefaultGadget(CardModel? choice)
    {
        //No card, no gadget
        if (choice is null)
        {
            return nameof(BrokenGadget);
        }

        //I don't plan on adding a lot of these, but if there's a really obvious gadget for a colorless card I can override the default logic
        string? specialCase = GetGadgetSpecialCase(choice);
        if (!string.IsNullOrEmpty(specialCase))
        {
            return specialCase;
        }

        //You aren't supposed to be able to scrap Statuses or Quests, but just in case we'll have them return the bad option.
        if (choice.Type is CardType.Curse or CardType.Status or CardType.Quest)
        {
            return nameof(CursedGadget);
        }

        //All ancient cards give you Battle Wrench. Not strictly the strongest, but consistently good.
        if (choice.Rarity == CardRarity.Ancient)
        {
            return nameof(BattleWrench);
        }

        //All of these dynamic vars are good indicators of the card's function, and have associated gadgets
        //Priority is determined by how likely the var is to be accurate and how unique the effect is
        DynamicVar? bestVar = choice.DynamicVars.Values.OrderBy(var => var switch //Lower value means higher priority var
        {
            PowerVar<PoisonPower> => 10,
            PowerVar<DoomPower> => 20,
            PowerVar<ThornsPower> => 30,
            PowerVar<VigorPower> => 40,
            PowerVar<VulnerablePower> => 50,
            PowerVar<WeakPower> => 60,
            EnergyVar => 70,
            CardsVar => 80,
            DamageVar or CalculatedDamageVar => 90,
            BlockVar or CalculatedBlockVar => 100,
            _ => 1000,
        }).FirstOrDefault();

        return bestVar switch
        {
            PowerVar<PoisonPower> => nameof(PoisonArrow),
            PowerVar<DoomPower> => nameof(OhNo),
            PowerVar<VigorPower> => nameof(PowerUp),
            PowerVar<ThornsPower> => nameof(Needle),
            PowerVar<VulnerablePower> => nameof(ShortCircuit),
            PowerVar<WeakPower> => nameof(Burrower),
            EnergyVar => nameof(MagicDice),
            CardsVar => choice.Rarity is CardRarity.Common or CardRarity.Basic ? nameof(MagicSpanner) : nameof(BattleWrench),
            DamageVar or CalculatedDamageVar => choice.TargetType == TargetType.AllEnemies ? nameof(Blowtorch) : nameof(Bonk),
            BlockVar or CalculatedBlockVar => choice.Rarity is CardRarity.Basic or CardRarity.Common ? nameof(Shield) : nameof(WallOfIce),
            _ => choice.Type switch
            {
                CardType.Attack => choice.TargetType is TargetType.AnyEnemy ? nameof(Bonk) : nameof(Blowtorch),
                CardType.Skill => choice.Rarity is CardRarity.Common or CardRarity.Basic ? nameof(Shield) : nameof(WallOfIce),
                CardType.Power => choice.Rarity is CardRarity.Uncommon ? nameof(MagicSpanner) : nameof(BattleWrench),
                CardType.Status or CardType.Curse or CardType.Quest => nameof(CursedGadget), //This branch shouldn't be reachable but just in case
                _ => nameof(BrokenGadget)
            }
        };
    }

    //If you add a special case, please include a justification
    private static string? GetGadgetSpecialCase(CardModel choice)
    {
        return choice switch
        {
            TheBomb => nameof(BigBomb), //It's called The Bomb, obviously it should give you a bigger bomb
            ByrdSwoop => nameof(ByrdNuggets), //Only gadget that is exclusive to an off class card, and it's for a dumb joke
            _ => null
        };
    }

    public static IEnumerable<string> GetRandomCombatGadgetId(Rng rng, int i)
    {
        return AllGadgets.Where(g => g.Value.IsAllowedAsTempGadget).TakeRandom(i, rng).Select(pair => pair.Key);
    }

    public static string GetRandomCombatGadgetId(Rng rng)
    {
        return GetRandomCombatGadgetId(rng, 1).Single();
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
            await parent.UpdateAndPreviewAsync();
        }
    }

    public static List<IGadgetParent> GetGadgetParents(Player owner) => [.. owner.RunState
        .IterateHookListeners(owner.Creature.CombatState)
        .OfType<IGadgetParent>()
        .Where(p => p.Owner == owner)];

    public void AfterCreatingNewRun(RunState runState, IReadOnlyList<Player> players, IReadOnlyList<ActModel> acts, IReadOnlyList<ModifierModel> modifiers,
        GameMode gameMode, int ascensionLevel, string seed)
    {
        foreach (Player p in players.Where(p => p.Character is TheInventor))
        {
            SetGadgetInfo(p, ascensionLevel > 5 ? nameof(Efficiency) : nameof(HeatRay), false);
        }
    }

    public static bool HasGadget(Player targetPlayer)
    {
        return GetGadgetParents(targetPlayer).Count > 0;
    }

    public static bool GadgetIsFromThisFloor(Player player)
    {
        return GadgetOrigin(player) == player.RunState.TotalFloor;
    }
}