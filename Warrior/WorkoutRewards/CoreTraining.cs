using BaseLib.Extensions;
using DiceTheSpire.Shared.DynamicVars;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Warrior.WorkoutRewards;

public class CoreTraining() : TheWarriorCard(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PlatingPower>(5M), .. RangeVars.Make(0, 0)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PlatingPower>(DynamicVars.Power<PlatingPower>().IntValue)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, DynamicVars.Power<PlatingPower>().IntValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Power<PlatingPower>().UpgradeValueBy(1);
    }

}
