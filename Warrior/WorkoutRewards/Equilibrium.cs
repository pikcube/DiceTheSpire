using BaseLib.Extensions;
using DiceTheSpire.Inventor.Powers;
using DiceTheSpire.Shared.DynamicVars;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;


namespace DiceTheSpire.Warrior.WorkoutRewards;


public class Equilibrium() : TheWarriorCard(3, CardType.Power, CardRarity.Token, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<EquilibriumPower>(30), .. RangeVars.Make(0, 2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<EquilibriumPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await EquilibriumPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Power<EquilibriumPower>().IntValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Power<EquilibriumPower>().UpgradeValueBy(10);
    }

}
