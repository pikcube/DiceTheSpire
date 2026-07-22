using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Keywords;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Uncommon;

public class Megabump() : TheInventorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
{
    public override string GetScrapId => nameof(AutoBump);

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Bump)];

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        List<Task> tasks = [];

        foreach (Player p in CombatState.Players)
        {
            BranchingPlayerChoiceContext bpcc = new(LocalContext.NetId ?? 0, GameActionType.CombatPlayPhaseOnly, choiceContext);
            tasks.Add(bpcc.AssignTaskAndWaitForPauseOrCompletion(DoBumpAsync(bpcc, p)));
        }

        await Task.WhenAll(tasks);
    }

    private async Task DoBumpAsync(PlayerChoiceContext choiceContext, Player p)
    {
        CardSelectorPrefs prefs = new(new LocString("card_selection", "TO_BUMP"), 1, 1);
        foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, p, prefs, null, this))
        {
            await card.BumpAsync(choiceContext);
        }
    }


    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
        AddKeyword(BlinkModel.Blink);
    }
}