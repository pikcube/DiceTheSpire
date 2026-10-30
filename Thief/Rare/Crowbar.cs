using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Thief.Rare;

public class Crowbar() : TheThiefCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override bool HasEnergyCostX => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CountdownModel.Countdown)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards)
        {
            if (card is not ICountdown countdown)
            {
                continue;
            }
            countdown.AddCountThisTurnOrUntilPlayed(-ResolveEnergyXValue());
            if (IsUpgraded)
            {
                card.EnergyCost.AddThisTurnOrUntilPlayed(-ResolveEnergyXValue());
            }
        }
    }
}