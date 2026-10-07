using BaseLib.Extensions;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.Uncommon;

public class FrozenSwordStrike() : TheWarriorCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8M, DamageProps.card), new PowerVar<FreezePower>(1), new PowerVar<NextTurnFuryPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FuryPower>(), HoverTipFactory.FromPower<FreezePower>(), HoverTipFactory.Static(StaticHoverTip.Block)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        await FreezePower.ApplyAsync(choiceContext, cardPlay.Target, 1, Owner.Creature, this);

        await PowerCmd.Apply<NextTurnFuryPower>(choiceContext, Owner.Creature, DynamicVars.Power<NextTurnFuryPower>().IntValue, Owner.Creature, this);

    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
    }
}