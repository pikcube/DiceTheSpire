using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Thief.Rare;

public class Approach() : TheThiefCard(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override TargetType TargetType => IsUpgraded ? TargetType.AllEnemies : TargetType.AnyEnemy;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<StrengthPower>(2), new PowerVar<VulnerablePower>(3)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<VulnerablePower>()
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars.Strength.BaseValue,
            Owner.Creature, this);
        if (IsUpgraded)
        {
            await PowerCmd.Apply<VulnerablePower>(choiceContext, CombatState.HittableEnemies,
                DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.BaseValue,
                Owner.Creature, this);
        }
    }
}