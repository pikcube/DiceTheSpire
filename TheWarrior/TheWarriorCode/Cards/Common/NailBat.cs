using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Common;

public class NailBat() : TheWarriorCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Recoil", 4), new DamageVar(14M, DamageProps.card)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Held)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }

    public override bool HasTurnEndInHandEffect => true;
    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {

        if (CombatState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars["Recoil"].IntValue)
            .FromCard(this, null)
            .Targeting(Owner.Creature)
            .WithValueProp(DynamicVars.Damage.Props)
            .WithHitFx(VfxCmd.rockShatterPath)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
        DynamicVars["Recoil"].UpgradeValueBy(2);
    }
}