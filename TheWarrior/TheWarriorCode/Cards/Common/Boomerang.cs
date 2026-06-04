using BaseLib.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;
namespace TheWarrior.TheWarriorCode.Cards.Common;


public class Boomerang() : TheWarriorCard(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8M, DamageProps.card), new RepeatVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
       .WithHitCount(DynamicVars.Repeat.IntValue)
       .FromCard(this)
       .TargetingAllOpponents(CombatState)
       .WithValueProp(DynamicVars.Damage.Props)
       .WithHitFx(VfxCmd.slashPath)
       .Execute(choiceContext);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
        .WithHitCount(DynamicVars.Repeat.IntValue)
        .FromCard(this)
        .Targeting(Owner.Creature)
        .WithValueProp(DynamicVars.Damage.Props)
        .WithHitFx(VfxCmd.slashPath)
        .Execute(choiceContext);
    }
    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(3);
    }

}
