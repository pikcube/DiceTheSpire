using DiceTheSpire.Common.Commands;
using DiceTheSpire.Common.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Common;

public class RummageStrike() : TheWarriorCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new DamageVar(6, DamageProps.card)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Rummage)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        await RummageCmd.RummageAsync(choiceContext, Owner, DynamicVars.Cards.IntValue, this);

        //CardSelectorPrefs cardSelectorPrefs = new(CardSelectorPrefs.DiscardSelectionPrompt, 0, DynamicVars.Cards.IntValue);
        //CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
        //foreach (CardModel card in cards)
        //{
        //    await CardCmd.Discard(choiceContext, card);
        //}

        //if (cards.Length == 0)
        //{
        //    return;
        //}
        //await CardPileCmd.Draw(choiceContext, cards.Length, Owner);

    }
    protected override void OnUpgrade()
    {
        base.OnUpgrade();
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars.Damage.UpgradeValueBy(2);
    }

}