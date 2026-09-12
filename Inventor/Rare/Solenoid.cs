using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Inventor.Rare;

public class Solenoid() : TheInventorCard(3, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
{
    public override string GetScrapId => nameof(BattleWrench);

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(6), new ShockVar(6)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(ShockModel.Shock)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null || RunState is null)
        {
            return;
        }
        foreach (Player p in CombatState.Players)
        {
            await CardPileCmd.DrawWithoutBlockingOnOtherPlayers(choiceContext, DynamicVars.Cards.IntValue, p, this);
            if (p != Owner)
            {
                continue;
            }

            CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToShock, DynamicVars.Shock.IntValue, DynamicVars.Shock.IntValue);
            IEnumerable<CardModel> results = await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this);
            await Task.WhenAll(results.Select(card => card.ShockAsync(choiceContext)));
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(3);
    }
}