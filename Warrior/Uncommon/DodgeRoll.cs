using DiceTheSpire.Shared.Cards;
using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Warrior.Uncommon
{
public class DodgeRoll() : TheWarriorCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {

        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), .. RangeVars.Make(0, 0)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Dodge), HoverTipFactory.FromCard<CombatRoll>(), HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {

            if (CombatState is null)
            {
                return;
            }


            for (int n = 0; n < DynamicVars.Cards.IntValue; ++n)
            {
                CombatRoll card = CombatState.CreateCard<CombatRoll>(Owner);
                //if (IsUpgraded)
                //{
                //    CardCmd.Upgrade(card);
                //}
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
            }

            await DodgeCmd.DodgeAsync(choiceContext, Owner, DynamicVars.Cards.IntValue, this);

        }

        protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1);

    }
}
