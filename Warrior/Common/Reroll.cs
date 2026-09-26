using DiceTheSpire.Shared.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Warrior.Common
{
    public class Reroll() : TheWarriorCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {

        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<RollAgain>(IsUpgraded)];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {

            if (CombatState is null)
            {
                return;
            }


            for (int n = 0; n < 3; ++n)
            {
                RollAgain card = CombatState.CreateCard<RollAgain>(Owner);
                if (IsUpgraded)
                {
                    CardCmd.Upgrade(card);
                }
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
            }

        }
    }
}
