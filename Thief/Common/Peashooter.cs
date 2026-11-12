using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Thief.Common;

public class Peashooter() : TheThiefCard(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy), ICountdown
{
    public int BaseCount
    {
        get;
        set;
    } = 0;

    public override bool HasCountdownCostX => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2M, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
            .WithHitCount(CountValue ?? 0).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2M);
    }
}