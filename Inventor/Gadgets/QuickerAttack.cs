using BaseLib.Abstracts;
using DiceTheSpire.Inventor.Basic;
using DiceTheSpire.Inventor.Uncommon;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Inventor.Gadgets;

public class QuickerAttack() : GadgetModel(nameof(QuickerAttack))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override async Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        if (Parent?.Owner != player || player.Creature.CombatState is null)
        {
            return;
        }

        CardModel? card = CardFactory.GetDistinctForCombat(player, player.Character.CardPool
                .GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
                .Where(c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable) && c is not RemoteControl),
            1, player.RunState.Rng.CombatCardGeneration).SingleOrDefault();

        card ??= player.Creature.CombatState.CreateCard<StrikeInventor>(player);

        Parent.Flash();
        await CardCmd.AutoPlay(choiceContext, card.CreateDupe(card.Owner), null);
    }
}