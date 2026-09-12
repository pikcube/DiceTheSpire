using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Warrior.Uncommon;
public class Tazer() : TheWarriorCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(10), new ShockVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Rummage), HoverTipFactory.FromKeyword(ShockModel.Shocked)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (RunState is null)
        {
            return;
        }

        await InventorHelperFunctions.ShockRandomAsync(choiceContext, Owner, RunState.Rng.CombatCardSelection, DynamicVars.Shock.IntValue);

        await RummageCmd.RummageAsync(choiceContext, Owner, DynamicVars.Cards.IntValue, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Shock.UpgradeValueBy(-1);
    }
}
