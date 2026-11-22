using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Inventor.Rare;

public class TabulaRasa() : TheInventorCard(-1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [ShockModel.Shock];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(ShockModel.Shock)];

    protected override bool HasEnergyCostX => true;

    public override string GetScrapId => nameof(Replicate);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int drawNum = cardPlay.Card.ResolveEnergyXValue();
        IReadOnlyList<CardModel> cardsInHand = PileType.Hand.GetPile(Owner).Cards;
        foreach (CardModel card in cardsInHand.ToArray())
        {
            await ShockModel.ShockCardAsync(choiceContext, card);
            await PlayerCmd.GainEnergy(card.EnergyCost.GetAmountToSpend(), Owner);
        }
        
        await CardPileCmd.Draw(choiceContext, drawNum, Owner);
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }
}