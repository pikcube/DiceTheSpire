using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;

[UsedImplicitly]
public class Fury() : GadgetModel(nameof(Fury))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;
    public override void OnFirstCharge()
    {
        Parent?.SetValue(Power);
    }

    private int Count { get; set; }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == CombatSide.Player)
        {
            Count = 0;
        }

        return Task.CompletedTask;
    }

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner != Parent?.Owner)
        {
            return playCount;
        }

        ++Count;
        Parent.SetValue(Count >= Power ? 0 : Power - Count);
        if (Count > Power || Count < 0)
        {
            return playCount;
        }

        Parent.Flash();

        return playCount + 1;
    }

    public override Task OnRechargeAsync(PlayerChoiceContext choiceContext, Player player)
    {
        Count = 0;
        return Task.CompletedTask;
    }
}