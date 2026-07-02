using System.Numerics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheThief.TheThiefCode.Powers;

namespace TheThief.TheThiefCode.Cards.Uncommon;

public class CactusNeedle() : TheThiefCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ThornsPower>(3), new CalculationBaseVar(9), new CalculationExtraVar(1), new CalculatedBlockVar(ValueProp.Move).WithMultiplier((card, _) => card.Owner.Creature.GetPowerAmount<ThornsPower>())];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ThornsPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        if (IsUpgraded)
        {
            await PowerCmd.Apply<CactusNeedlePower>(choiceContext, Owner.Creature, DynamicVars["ThornsPower"].BaseValue,
                Owner.Creature, this);
        }
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(Owner.Creature), ValueProp.Move, cardPlay);
        
    }
}