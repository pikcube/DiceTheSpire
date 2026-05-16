using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInventor.TheInventorCode.Gadgets;

//Starting Gadget.
//Probably shouldn't give this to the player after the first combat unless they scrap something insanely strong since this gadget is busted.
[UsedImplicitly]
public class HeatRay() : AbstractGadget(nameof(HeatRay))
{
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
            
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireBurstVfx.Create(creature, 0.75f));
            SfxCmd.Play("event:/sfx/characters/attack_fire");
            await CreatureCmd.Damage(choiceContext, creature, 5, DamageProps.nonCardUnpowered, null, null);
        }
    }
}