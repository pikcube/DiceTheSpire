using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheThief.TheThiefCode.Cards.Uncommon;

public class ToxicClaw() : TheThiefCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    private Decimal _extraDamageFromClawPlays;
    private Decimal ExtraDamageFromClawPlays
    {
        get => _extraDamageFromClawPlays;
        set
        {
            AssertMutable();
            _extraDamageFromClawPlays = value;
        }
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(5M, ValueProp.Move), new PowerVar<PoisonPower>(2M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PoisonPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath).Execute(choiceContext);
        await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, DynamicVars.Poison.BaseValue,
            Owner.Creature, this);
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card == this)
        {
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CreateClone(), PileType.Hand, card.Owner));
        }
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card is Claw && cardPlay.Card.Owner == Owner)
        {
            BuffFromClawPlay(cardPlay.Card.DynamicVars["Increase"].BaseValue);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Poison.UpgradeValueBy(1);
    }

    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + ExtraDamageFromClawPlays;
    }

    private void BuffFromClawPlay(Decimal extraDamage)
    {
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + extraDamage;
        ExtraDamageFromClawPlays += extraDamage;
    }
}