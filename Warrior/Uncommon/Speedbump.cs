using BaseLib.Extensions;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Warrior.Uncommon;
public class Speedbump() : TheWarriorCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)//, IRangeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new PowerVar<SpeedbumpPower>(1M)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Bump)]; //BetterStaticHoverTips.RangeHoverTip(this),
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<SpeedbumpPower>(choiceContext, Owner.Creature, DynamicVars.Power<SpeedbumpPower>().IntValue, Owner.Creature, this);

        //var cards = 
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        //foreach (CardModel card in cards)
        //{
            //await card.BumpAsync(choiceContext);
            //if(IsUpgraded)
            //{
            //    if(card.EnergyCost.CostsX==false)
            //    {
            //        card.EnergyCost.SetThisTurnOrUntilPlayed(card.EnergyCost.GetAmountToSpend() - 1);
            //    }
            //}
        //}
    }

    //public int MinimumCost => 0;
    //public int MaximumCost => 0;
    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}




