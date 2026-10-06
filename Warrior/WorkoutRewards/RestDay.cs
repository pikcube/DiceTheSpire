using BaseLib.Extensions;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Warrior.WorkoutRewards
{
    public class RestDay() : TheWarriorCard(2, CardType.Skill, CardRarity.Token, TargetType.AnyEnemy)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RestDayStrengthDownPower>(99)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            await PowerCmd.Apply<RestDayStrengthDownPower>(choiceContext, cardPlay.Target, DynamicVars.Power<RestDayStrengthDownPower>().IntValue, Owner.Creature, cardPlay.Card);

        }
        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}


