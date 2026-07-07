using DiceTheSpireCore.DiceTheSpireCoreCode;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.TestSupport;



namespace TheWarrior.TheWarriorCode.Cards.Uncommon;

public class MysteryBox() : TheWarriorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];

    public int TestEnergyCostOverride
    {
        get;
        set
        {
            TestMode.AssertOn();
            AssertMutable();
            field = value;
        }
    } = -1;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        var cards = await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        foreach (CardModel card in cards)
        {
            if (card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
            {
                card.EnergyCost.SetThisTurnOrUntilPlayed(NextEnergyCost());
                NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
            }
        }
    }
    private int NextEnergyCost()
    {
        return TestEnergyCostOverride >= 0 ? TestEnergyCostOverride : Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}