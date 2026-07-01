using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheThief.TheThiefCode.Powers;

namespace TheThief.TheThiefCode.Cards.Uncommon;

public class WreckingBall() : TheThiefCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WreckingBallPower>(4)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Pip>()];

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