using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Thief.Uncommon;

public class Crowbar() : TheThiefCard(3, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CountdownModel.Countdown)];

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return Task.CompletedTask;
        }

        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards)
        {
            if (card is ICountdown)
            {
                card.SetToFreeThisTurn();
            }
        }

        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}