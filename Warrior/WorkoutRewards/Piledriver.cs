using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.WorkoutRewards;
public class Piledriver() : TheWarriorCard(3, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(30M, DamageProps.card), new PowerVar<VulnerablePower>(5)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.bluntPath)
            .Execute(choiceContext);

        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.IntValue,
            Owner.Creature, cardPlay.Card);

    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
        DynamicVars.Vulnerable.UpgradeValueBy(2);

    }

}
