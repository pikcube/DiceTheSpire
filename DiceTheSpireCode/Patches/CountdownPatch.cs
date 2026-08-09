using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;

namespace DiceTheSpire.DiceTheSpireCode.Patches;

[HarmonyPatch(typeof(CardModel), nameof(CardModel.OnPlayWrapper))]
public static class CountdownPatch
{
    //All branches eventually execute the OnPlayWrapper, the question is whether there is any sandwich logic happening before and after or not.
    public static bool Prefix(ref Task __result, CardModel __instance, PlayerChoiceContext choiceContext, Creature? target,
        bool isAutoPlay, ResourceInfo resources, bool skipCardPileVisuals = false)
    {
        //Don't touch it if it isn't a countdown.
        if (__instance is not ICountdown countdown)
        {
            return true;
        }

        __result = OnPlayWrapper(__instance, countdown, choiceContext, target, isAutoPlay, resources, skipCardPileVisuals);
        return false;
    }

    public static async Task OnPlayWrapper(CardModel instance, ICountdown countdown, PlayerChoiceContext choiceContext, Creature? target, bool isAutoPlay, ResourceInfo resources, bool skipCardPileVisuals = false)
    {
        if (instance.CombatState is null)
        {
            return;
        }
        choiceContext.PushModel(instance);
        await CombatManager.Instance.WaitForUnpause();
        PrivatePropertyWrapper<CardModel, Creature> currentTarget = instance.PrivatePropertyWrapper<CardModel, Creature>("CurrentTarget");
        PrivatePropertyWrapper<CardModel, int> currentPlayIndex =
            instance.PrivatePropertyWrapper<CardModel, int>("CurrentPlayIndex");

        currentTarget.Value = target;
        currentPlayIndex.Value = 0;
        if (!isAutoPlay)
        {
            await CardPileCmd.AddDuringManualCardPlay(instance);
        }
        else
        {
            await CardPileCmd.Add(instance, PileType.Play, CardPilePosition.Bottom, null, skipCardPileVisuals);
            if (!skipCardPileVisuals)
            {
                await Cmd.CustomScaledWait(0.25f, 0.35f);
            }

            while (countdown.CurrentCount > 0)
            {
                await countdown.DecrementCountAsync();
            }
        }

        ICombatState combatState = instance.CombatState;
        if (combatState == null)
        {
            return;
        }
        CardLocation resultLocation = countdown.PublicGetResultLocationForCardPlay();
        resultLocation = Hook.ModifyCardPlayResultLocation(combatState, instance, isAutoPlay, resources, resultLocation, out IEnumerable<AbstractModel> modifiers);
        foreach (AbstractModel item in modifiers)
        {
            await item.AfterModifyingCardPlayResultLocation(instance, resultLocation);
        }

        IEnumerable<CardModel> cards = await CardSelectCmd.FromHandForDiscard(choiceContext, instance.Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt,
                0, countdown.CurrentCount), null, instance);
        CardModel[] cardsDiscarded = [..cards];
        await CardCmd.Discard(choiceContext, cardsDiscarded);
        await countdown.DecrementCountAsync(cardsDiscarded.Length);
        if (countdown.CurrentCount > 0)
        {
            await CardPileCmd.Add(instance, PileType.Discard);
            return;
        }
        int playCount = await countdown.PublicGeneratePlayCount(combatState, target);
        if (instance.Owner.Creature.IsDead)
        {
            return;
        }
        ulong playStartTime = Time.GetTicksMsec();
        CombatId? effectCombatId = CombatManager.Instance.BeginCardOrPotionEffect(instance.Owner);
        try
        {
            for (int i = 0; i < playCount; i++)
            {
                if (CombatManager.Instance.IsOverOrEnding)
                {
                    break;
                }
                currentPlayIndex.Value = i;
                if (instance.Type == CardType.Power)
                {
                    await ((Task?)AccessTools.DeclaredMethod(typeof(CardModel), "PlayPowerCardFlyVfx")
                        .Invoke(instance, []) ?? Task.CompletedTask);
                }
                else if (i > 0)
                {
                    NCard? nCard = NCard.FindOnTable(instance);
                    if (nCard != null)
                    {
                        await nCard.AnimMultiCardPlay();
                    }
                }
                CardPlay cardPlay = new()
                {
                    Card = instance,
                    Player = instance.Owner,
                    Target = target,
                    ResultPile = resultLocation.pileType,
                    Resources = resources,
                    IsAutoPlay = isAutoPlay,
                    PlayIndex = i,
                    PlayCount = playCount
                };
                await Hook.BeforeCardPlayed(combatState, cardPlay);
                CombatManager.Instance.History.CardPlayStarted(combatState, cardPlay);
                BranchingPlayerChoiceContext branchingPlayerChoiceContext = new(instance, LocalContext.NetId ?? 0, GameActionType.Combat, choiceContext);
                branchingPlayerChoiceContext.PushModel(instance);
                Task task = countdown.PublicOnPlay(branchingPlayerChoiceContext, cardPlay);
                await branchingPlayerChoiceContext.AssignTaskAndWaitForPauseOrCompletion(task);
                if (instance.Owner.Creature.IsDead)
                {
                    return;
                }
                instance.InvokeExecutionFinished();
                if (instance.Enchantment != null)
                {
                    await instance.Enchantment.OnPlay(choiceContext, cardPlay);
                    if (instance.Owner.Creature.IsDead)
                    {
                        return;
                    }
                    instance.Enchantment.InvokeExecutionFinished();
                }
                if (instance.Affliction != null)
                {
                    AfflictionModel affliction = instance.Affliction;
                    await affliction.OnPlay(choiceContext, target);
                    if (instance.Owner.Creature.IsDead)
                    {
                        return;
                    }
                    affliction.InvokeExecutionFinished();
                }
                CombatManager.Instance.History.CardPlayFinished(combatState, cardPlay);
                if (CombatManager.Instance.IsInProgress)
                {
                    await Hook.AfterCardPlayed(combatState, choiceContext, cardPlay);
                    if (instance.Owner.Creature.IsDead)
                    {
                        return;
                    }
                }
            }
        }
        finally
        {
            await CombatManager.Instance.EndCardOrPotionEffect(effectCombatId, instance.Owner);
        }
        if (!skipCardPileVisuals)
        {
            float num = (Time.GetTicksMsec() - playStartTime) / 1000f;
            await Cmd.CustomScaledWait(0.15f - num, 0.3f - num);
        }
        Player originalOwner = instance.Owner;
        if (originalOwner != resultLocation.player && resultLocation.pileType != PileType.None)
        {
            await CardPileCmd.GiveToAnotherPlayer(instance, resultLocation.player, resultLocation.pileType, resultLocation.position);
        }
        CardPile? pile = instance.Pile;
        if (pile is { Type: PileType.Play })
        {
            switch (resultLocation.pileType)
            {
                case PileType.None:
                    await CardPileCmd.RemoveFromCombat(instance, skipCardPileVisuals);
                    break;
                case PileType.Exhaust:
                    await CardCmd.Exhaust(choiceContext, instance, causedByEthereal: false, skipCardPileVisuals);
                    break;
                case PileType.Draw:
                case PileType.Hand:
                case PileType.Discard:
                case PileType.Play:
                case PileType.Deck:
                default:
                    await CardPileCmd.Add(instance, resultLocation.pileType, resultLocation.position, null, skipCardPileVisuals);
                    break;
            }
        }
        await CombatManager.Instance.CheckForEmptyHand(effectCombatId, choiceContext, originalOwner);
        if (instance.EnergyCost.AfterCardPlayedCleanup())
        {
            instance.InvokeEnergyCostChanged();
        }
        currentTarget.Value = null;
        currentPlayIndex.Value = 0;
        AccessTools.DeclaredEvent(typeof(CardModel), nameof(CardModel.Played)).GetRaiseMethod(true)?.Invoke(instance, []);
        countdown.ResetCount();
        choiceContext.PopModel(instance);
    }
}