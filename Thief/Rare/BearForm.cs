using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.Thief.Rare;

public class BearForm() : TheThiefCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<BearFormReplayPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}