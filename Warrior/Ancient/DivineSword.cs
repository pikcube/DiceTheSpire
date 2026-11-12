using BaseLib.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Ancient;
public class DivineSword() : TheWarriorCard(2, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy), IFuryModifier
{
    public bool ShouldIgnoreFury => true;
    public bool ShouldMaintainFury => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(20M, DamageProps.card),
        new PowerVar<FuryPower>(3M)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FuryPower>(DynamicVars.Power<FuryPower>().IntValue)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        await PowerCmd.Apply<FuryPower>(choiceContext, Owner.Creature, DynamicVars.Power<FuryPower>().IntValue, Owner.Creature, this);

    }
    protected override void OnUpgrade()
    {
        DynamicVars.Power<FuryPower>().UpgradeValueBy(1);
    }

}