using System.Data;
using BaseLib.Abstracts;
using DiceTheSpire.Shared.Patches;
using HarmonyLib;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Gadgets;

[UsedImplicitly]
public class Recyclomancy() : GadgetModel(nameof(Recyclomancy))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Run;

    public override bool IsAllowedAsTempGadget => false;

    public override bool ModifiesRewards => true;

    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != Parent?.Owner)
        {
            return false;
        }

        if (AccessTools.DeclaredProperty(typeof(TrashHeap), "Cards").GetMethod?.Invoke(null, []) is not CardModel[] trashHeap)
        {
            throw new NoNullAllowedException();
        }

        List<CardModel> deck = [..trashHeap];

        //Now we can actually generate the version that can go into your deck
        for (int n = 0; n < deck.Count; ++n)
        {
            CardModel card = deck[n];
            card = card.StrongMutableClone();
            card.Owner = null!;
            card.Owner = player;
            deck[n] = player.RunState.CloneCard(card);
        }

        player.PlayerRng.Rewards.Shuffle(deck);

        List<CardModel> backupCards = deck[3..];

        CardReward cardReward = new(deck[..3], CardCreationSource.Encounter, player, new CardCreationOptions([], CardCreationSource.Encounter, CardRarityOddsType.Uniform));
        cardReward.Populate();
        rewards.Add(cardReward);
        CardRewardRerollPatch.HijackReroll(cardReward, backupCards);

        BreakMe();
        return true;
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player) => Task.CompletedTask;
}