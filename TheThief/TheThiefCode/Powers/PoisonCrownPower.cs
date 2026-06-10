using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheThief.TheThiefCode.Powers;

public class PoisonCrownPower : TheThiefPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator is null || creator != Owner.Player || card is not Pip)
        {
            return;
        }
        await PowerCmd.Apply<PoisonPower>(new ThrowingPlayerChoiceContext(), CombatState.Enemies, Amount, Owner, null);
    }
}