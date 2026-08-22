using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Commands;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Keywords;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Rare;

public class ConfusingCrystal() : TheWarriorCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ReturnModel.Return];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(3)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards.Where(c => !c.EnergyCost.CostsX))
        {
            if ((!IsUpgraded && (card.EnergyCost.GetWithModifiers(CostModifiers.None) < 0 || card.EnergyCost.GetAmountToSpend() < 3)) || (IsUpgraded && (card.EnergyCost.GetWithModifiers(CostModifiers.None) < 0 || card.EnergyCost.GetAmountToSpend() < 2)))
            {
                continue;
            }

            NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
            await RerollCmd.RerollAsync(card, RerollDuration.UntilEndOfTurnOrPlayed);
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(-1);
    }

}