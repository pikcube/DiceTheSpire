using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Common.Commands;
using DiceTheSpire.DiceTheSpireCode.Common.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace DiceTheSpire.DiceTheSpireCode.Warrior.Uncommon;
public class TableSlam() : TheWarriorCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RetainHandPower>(1M)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards.Where(c => !c.EnergyCost.CostsX))
        {
            if (card.EnergyCost.GetWithModifiers(CostModifiers.None) < 0)
            {
                continue;
            }

            NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
            await RerollCmd.RerollAsync(card, RerollDuration.UntilEndOfTurnOrPlayed);
        }
        await PowerCmd.Apply<RetainHandPower>(choiceContext, Owner.Creature, DynamicVars.Power<RetainHandPower>().BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}