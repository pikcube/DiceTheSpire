using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheThief.TheThiefCode.Cards.Uncommon;

  
public class PoisonNeedle() : TheThiefCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override bool HasEnergyCostX => true;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PoisonPower>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PoisonPower>(3)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        int xValue = ResolveEnergyXValue();

        for (int i = 0; i < xValue; ++i)
        {
            await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, DynamicVars.Poison.BaseValue,
                Owner.Creature, this);
            await Cmd.Wait(0.2f);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Poison.UpgradeValueBy(1);
    }
}