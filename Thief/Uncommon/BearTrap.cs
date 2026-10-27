using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Thief.Uncommon;

public class BearTrap() : TheThiefCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy), ICountdown
{
    public int BaseCount { get; set; } = 1;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, -DynamicVars.Strength.BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Strength.UpgradeValueBy(1);
    }
}