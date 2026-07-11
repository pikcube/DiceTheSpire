using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInventor.TheInventorCode.Gadgets;

public class Overload() : GadgetModel(nameof(Overload))
{
    public override CustomSingletonModel.HookType HookType => CustomSingletonModel.HookType.Combat;

    public override void OnFirstCharge()
    {
        Parent?.SetValue(Power);
    }

    private int Count { get; set; }
    public override Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        Count = 0;
        return Task.CompletedTask;
    }

    public override decimal ModifyBlockMultiplicative(Creature target, decimal block, ValueProp props,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (Parent is null || Count > Power || cardPlay is null || cardPlay.Card.Owner != Parent.Owner)
        {
            return 1;
        }

        Parent?.Flash();
        return 2;
    }

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props,
        Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay)
    {
        if (Parent is null || Count > Power || cardSource is null || cardSource.Owner != Parent.Owner)
        {
            return 1;
        }

        Parent?.Flash();
        return 2;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Parent?.Owner)
        {
            return Task.CompletedTask;
        }

        ++Count;
        Parent.SetValue(Power - Count > 0 ? Power - Count : 0);

        return Task.CompletedTask;
    }

    public override bool IsAllowedAsTempGadget => false;
}