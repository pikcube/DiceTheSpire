using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Thief.Common;

public class RainbowStrike() : TheThiefCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy), ICountdown
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    public int BaseCount { get; set; } = 1;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        ..MakeCalculatedDamage(6, (card, _) =>
        {
            return card.Owner.Deck.Cards.DistinctBy(c => c.Pool.Id).Count();
        }, 2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(1);
    }
}