using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Common;


public class Boomerang() : TheWarriorCard(3, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10M, DamageProps.card), new IntVar("Recoil", 6), new RepeatVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState)
            .WithValueProp(DynamicVars.Damage.Props)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        await DamageCmd.Attack(DynamicVars["Recoil"].IntValue)
            .FromCard(this, cardPlay)
            .Targeting(Owner.Creature)
            .WithValueProp(DynamicVars.Damage.Props)
            .WithHitFx(VfxCmd.rockShatterPath)
            .Execute(choiceContext);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
        DynamicVars["Recoil"].UpgradeValueBy(2);
    }

}