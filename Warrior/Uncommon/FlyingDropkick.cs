using BaseLib.Extensions;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Uncommon;


public class FlyingDropkick() : TheWarriorCard(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FuryPower>(6M), new DamageVar(10M, DamageProps.card)];
    //new IntVar("FuryBonus", 1)
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FuryPower>(DynamicVars.Power<FuryPower>().IntValue)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        CardModel[] cards = [..PileType.Hand.GetPile(Owner).Cards];

        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .WithHitCount(1)
            .FromCard(this, cardPlay)
            .WithHitFx(VfxCmd.slashPath)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        await PowerCmd.Apply<FuryPower>(choiceContext, Owner.Creature, (cards.Length == 0 ? DynamicVars.Power<FuryPower>().IntValue : 0), Owner.Creature, this);

    }

    //protected override bool ShouldGlowGoldInternal => Owner is null ? false : ([..PileType.Hand.GetPile(Owner).Cards?.Count] == 1 ? true : false);
    protected override void OnUpgrade()
    {
        DynamicVars.Power<FuryPower>().UpgradeValueBy(2);
    }
}