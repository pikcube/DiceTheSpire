using DiceTheSpireCore.DiceTheSpireCoreCode;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
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

        Dictionary<Player, Task<IEnumerable<CardModel>>> results = [];

        foreach (Player p in CombatState.Players)
        {
            CardSelectorPrefs prefs = new(new LocString("card_selection", "TO_BUMP"), 1, 1);
            results.Add(p, CardSelectCmd.FromHand(choiceContext, p, prefs, null, this));
        }

        await Task.WhenAll(results.Values);

        foreach ((Player p, Task<IEnumerable<CardModel>> value) in results)
        {
            CardModel[] r = [.. await value];
            foreach (CardModel c in r)
            {
                await c.BumpAsync(choiceContext);
            }
        }
    }


    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
        AddKeyword(BlinkModel.Blink);
    }
}