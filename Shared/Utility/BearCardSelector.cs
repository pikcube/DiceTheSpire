using DiceTheSpire.Shared.Cards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace DiceTheSpire.Shared.Utility;

public class BearCardSelector : ICardSelector
{
    public Task<IEnumerable<CardModel>> GetSelectedCards(IEnumerable<CardModel> options, int minSelect, int maxSelect)
    {
        return Task.FromResult(options.Take(maxSelect));
    }

    public CardRewardSelection GetSelectedCardReward(IReadOnlyList<CardCreationResult> options, IReadOnlyList<CardRewardAlternative> alternatives)
    {
        if (options.Count == 0)
        {
            return new CardRewardSelection();
        }

        return new CardRewardSelection
        {
            card = options[0].Card
        };
    }

    public static async Task DoBearTurnSelection(PlayerChoiceContext choiceContext, Player player, int count)
    {
        if (player.Creature.CombatState is null)
        {
            return;
        }

        using IDisposable _ = CardSelectCmd.PushSelector(new BearCardSelector());

        while (count > 0 && !CombatManager.Instance.IsOverOrEnding && !CombatManager.Instance.IsPlayerReadyToEndTurn(player))
        {
            --count;
            CardModel? card = player.PlayerCombatState?.Hand.Cards.OrderByDescending(c => c.Type == CardType.Attack).FirstOrDefault();
            if (card is null)
            {
                return;
            }

            await card.SpendResources();
            if (card is DiceTheSpireCard diceyCard)
            {
                await diceyCard.DiceySpendResourcesAsync(choiceContext);
            }

            Creature? target = GetTarget(player, card, player.Creature.CombatState);

            await CardCmd.AutoPlay(choiceContext, card, target, skipXCapture: true);
        }

    }

    private static Creature? GetTarget(Player player, CardModel card, ICombatState combatState)
    {
        Rng combatTargets = player.RunState.Rng.CombatTargets;
        Creature? target = card.TargetType switch
        {
            TargetType.AnyEnemy => combatState.HittableEnemies.MinBy(c => c.CurrentHp),
            TargetType.AnyPlayer => player.Creature,
            TargetType.AnyAlly => combatTargets.NextItem(combatState.Allies.Where(c =>
                c is { IsAlive: true, IsPlayer: true } && c != player.Creature)),
            _ => null
        };
        return target;
    }
}