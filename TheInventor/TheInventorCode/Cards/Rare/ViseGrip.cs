using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Keywords;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class ViseGrip() : TheInventorCard(-1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BlinkModel.Blink];

    protected override bool HasEnergyCostX => true;

    public override string GetScrapId => nameof(BattleWrench);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int drawNum = cardPlay.Card.ResolveEnergyXValue();
        if (IsUpgradable)
        {
            ++drawNum;
        }
        IReadOnlyList<CardModel> cardsInHand = PileType.Hand.GetPile(Owner).Cards;
        int energyGain = cardsInHand.Count;
        foreach (CardModel card in cardsInHand)
        {
            await BlinkModel.BlinkCardAsync(choiceContext, card);
        }

        await CardPileCmd.Draw(choiceContext, drawNum, Owner);
        await PlayerCmd.GainEnergy(energyGain, Owner);
    }
}