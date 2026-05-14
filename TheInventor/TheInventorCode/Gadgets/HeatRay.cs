using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInventor.TheInventorCode.Gadgets;

public class HeatRay() : AbstractGadget("Gadget.HeatRay")
{
    public override StringVar GadgetName => new(nameof(GadgetName), "Heat Ray");

    public override StringVar GadgetDescription =>
        new StringVar(nameof(GadgetDescription), "Whenever you play a card, all enemies take 5 damage");

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Parent?.Owner)
        {
            return;
        }

        ICombatState? creatureCombatState = cardPlay.Card.Owner.Creature.CombatState;
        
        if (creatureCombatState is null)
        {
            return;
        }

        foreach (Creature creature in creatureCombatState.Enemies.ToArray())
        {
            await CreatureCmd.Damage(choiceContext, creature, 5, ValueProp.Unpowered, null, null);
        }
    }
}