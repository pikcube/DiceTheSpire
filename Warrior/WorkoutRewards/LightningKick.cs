using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.WorkoutRewards;
public class LightningKick() : TheWarriorCard(2, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(1M, DamageProps.card), new RepeatVar(8)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        if (CombatState is null)
        {
            return;
        }


        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
        .WithHitCount(DynamicVars.Repeat.IntValue)
        .FromCard(this, cardPlay)
        .Targeting(cardPlay.Target)
        .WithHitFx(VfxCmd.slashPath)
        .Execute(choiceContext);
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(2);
    }

}
