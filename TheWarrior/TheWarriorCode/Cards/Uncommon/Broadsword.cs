using DiceTheSpireCore.DiceTheSpireCoreCode;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon
{

    public class Broadsword() : TheWarriorCard(3, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(6), new DamageVar(16, DamageProps.card)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Rummage)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (CombatState is null)
            {
                return;
            }
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(CombatState)
                .WithHitFx(VfxCmd.slashPath)
                .Execute(choiceContext);

            CardSelectorPrefs cardSelectorPrefs = new(new LocString("card_selection", "TO_DISCARD"), 0, DynamicVars.Cards.IntValue);
            CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
            foreach (CardModel card in cards)
            {
                await CardCmd.Discard(choiceContext, card);
            }

            if (cards.Length == 0)
            {
                return;
            }
            await CardPileCmd.Draw(choiceContext, cards.Length, Owner);

        }
        protected override void OnUpgrade()
        {
            base.OnUpgrade();
            DynamicVars.Cards.UpgradeValueBy(4);
            DynamicVars.Damage.UpgradeValueBy(4);
        }

    }

}
