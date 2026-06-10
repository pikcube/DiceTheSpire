using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheWarrior.TheWarriorCode.Cards.Rare
{

    public class CrystalSword() : TheWarriorCard(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {

        protected override IEnumerable<DynamicVar> CanonicalVars => [.. MakeCalculatedDamage(0, Bonus, 1)];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust,CardKeyword.Ethereal];

        private static decimal Bonus(CardModel card, Creature? arg2)
        {

            if (card.Owner.PlayerCombatState is null)
            {
                return 0;
            }

            int damage = 0;

            List<CardModel> AllCards = [.. card.Owner.PlayerCombatState.AllCards];
            foreach (CardModel c in AllCards)
            {
                int xValue = c.EnergyCost.GetAmountToSpend();

                damage += xValue;
            }
            return damage;
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner.PlayerCombatState is null || cardPlay.Target is null)
            {
                return;
            }

            await DamageCmd.Attack(DynamicVars.CalculatedDamage)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx(VfxCmd.slashPath)
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Ethereal);
        }

    }
}