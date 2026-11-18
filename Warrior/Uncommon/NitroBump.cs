using BaseLib.Extensions;
using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Warrior.Uncommon;

public class NitroBump() : TheWarriorCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self), IFuryModifier
{
    public bool ShouldIgnoreFury => true;
    public bool ShouldMaintainFury => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FuryPower>(1M)]; //new CardsVar(2),
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Bump), HoverTipFactory.FromPower<FuryPower>(DynamicVars.Power<FuryPower>().IntValue)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FuryPower>(choiceContext, Owner.Creature, DynamicVars.Power<FuryPower>().IntValue, Owner.Creature, this);

        //CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToBump, 0, DynamicVars.Cards.IntValue);
        //CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
        //foreach (CardModel card in cards)
        //{
        //    await card.BumpAsync(choiceContext);
        //}

        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards)
        {
            await BumpCmd.BumpAsync(choiceContext, card);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

}