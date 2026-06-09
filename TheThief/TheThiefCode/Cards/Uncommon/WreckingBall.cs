using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheThief.TheThiefCode.Powers;

namespace TheThief.TheThiefCode.Cards.Uncommon;

public class WreckingBall() : TheThiefCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2, ValueProp.Unblockable|ValueProp.Unpowered), new PowerVar<WreckingBallPower>(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<WreckingBallPower>(choiceContext, Owner.Creature,
            DynamicVars["WreckingBallPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["WreckingBallPower"].UpgradeValueBy(1);
    }
}