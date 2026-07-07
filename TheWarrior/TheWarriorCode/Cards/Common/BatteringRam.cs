using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheWarrior.TheWarriorCode.Cards.Common
{

    public class BatteringRam() : TheWarriorCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [.. MakeCalculatedDamage(0, Bonus)];

        private static decimal Bonus(CardModel card, Creature? arg2)
        {

            if (card.Owner.PlayerCombatState is null)
            {
                return 0;
            }

            int damage = card.Owner.Creature.Block * 2;

            return damage;
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner.PlayerCombatState is null || cardPlay.Target is null)
            {
                return;
            }

            await DamageCmd.Attack(DynamicVars.CalculatedDamage)
                .FromCard(this, cardPlay)
                .Targeting(cardPlay.Target)
                .WithHitFx(VfxCmd.slashPath)
                .Execute(choiceContext);

            await CreatureCmd.LoseBlock(Owner.Creature, Owner.Creature.Block);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}