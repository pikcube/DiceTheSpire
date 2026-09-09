using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Thief.Rare;

public class EchoBlast() : TheThiefCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar("SingleDamage", 10, ValueProp.Move), new DamageVar("AreaDamage", 5, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        if (CombatState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars["SingleDamage"].EnchantedValue).Targeting(cardPlay.Target)
            .FromCard(this, cardPlay).Execute(choiceContext);

        await DamageCmd.Attack(DynamicVars["AreaDamage"].EnchantedValue).FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["SingleDamage"].UpgradeValueBy(2);
        DynamicVars["AreaDamage"].UpgradeValueBy(1);
    }
}