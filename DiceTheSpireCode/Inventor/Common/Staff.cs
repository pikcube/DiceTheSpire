using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Common.Powers;
using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Common;


public class Staff() :TheInventorCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12, DamageProps.card), new PowerVar<ExhaustionPower>(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithValueProp(DynamicVars.Damage.Props)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }

    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card == this)
        {
            await PowerCmd.Apply<ExhaustionPower>(choiceContext, Owner.Creature, DynamicVars.Power<ExhaustionPower>().IntValue,
                Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<ExhaustionPower>().UpgradeValueBy(-1);
    }

    public override string GetScrapId => nameof(Blowtorch);
}