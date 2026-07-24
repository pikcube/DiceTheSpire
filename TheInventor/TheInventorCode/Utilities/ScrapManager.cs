using BaseLib.Abstracts;
using BaseLib.Utils;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
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
using TheInventor.TheInventorCode.Cards;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Interfaces;
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

    public static readonly SavedSpireField<Player, string> GadgetId = new(() => nameof(DefaultGadget), $"{MainFile.ModId}_{nameof(GadgetId)}");

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

    public static bool ScrapComplete(Player player)
    {
        return Ignore.Contains(player);
    }

    public override Task BeforeRoomEntered(AbstractRoom room)
    {
        Ignore.Clear();
        return Task.CompletedTask;
    }

    public static async Task DoScrapAsyncFor(RewardsSet rewardsSet)
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
                GadgetId.Set(p, nameof(BrokenGadget));
            }

            CardModel[] choiceClones = [.. cardModels.Select(c => (CardModel)c.ClonePreservingMutability())];

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
                await CardPileCmd.RemoveFromDeck(choice);
            }

            if (choice is TheInventorCard scrapCard)
            {
                if (!scrapCard.ModifyScrap())
                {
                    GadgetId.Set(p, scrapCard.GetScrapId);
                    await scrapCard.OnScrapAsync();
                    TempParent parent = new(p, AllGadgets[scrapCard.GetScrapId]);
                    await parent.LinkedGadgetModel.OnPickupAsync();
                    parent.LinkedGadgetModel.TryModifyRewards(p, rewardsSet.Rewards, p.RunState.CurrentRoom);
                }
            }
            else
            {
                string gadgetId = GetDefaultGadget(choice);
                GadgetId.Set(p, gadgetId);
                TempParent parent = new(p, AllGadgets[gadgetId]);
                await parent.LinkedGadgetModel.OnPickupAsync();
            }

            Ignore.Add(rewardsSet.Player);
        }
        finally
        {
            if (LocalContext.IsMe(rewardsSet.Player))
            {
                NMapScreen.Instance.SetTravelEnabled(canTravel);
            }
        }

        await rewardsSet.Offer();
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

    public static bool HasGadget(Player targetPlayer)
    {
        return GetGadgetParents(targetPlayer).Count > 0;
    }
}