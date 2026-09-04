using DiceTheSpire.Shared.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Uncommon;

public class HauntedSword() : TheWarriorCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4M, DamageProps.card), new CardsVar(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<RollAgain>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        if (CombatState is null)
        {
            return;
        }

        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
        if (IsUpgraded)
        {
            await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
            await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
        }

    }

    protected override void OnUpgrade()
    {
        base.OnUpgrade();
        DynamicVars.Cards.UpgradeValueBy(2);
    }

}