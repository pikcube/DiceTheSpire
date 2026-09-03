using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Thief.Common;

public class Wail() : TheThiefCard(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(5, ValueProp.Move), new PowerVar<WeakPower>(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(CombatState)
            .Execute(choiceContext);
        await PowerCmd.Apply<WeakPower>(choiceContext, CombatState.HittableEnemies, DynamicVars.Weak.BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}