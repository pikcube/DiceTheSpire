using DiceTheSpire.Common.Commands;
using DiceTheSpire.Common.Extensions;
using DiceTheSpire.Common.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Warrior.Common;

public class SineWave() : TheWarriorCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Nudge), HoverTipFactory.Static(BetterStaticHoverTips.Bump)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null || Owner.PlayerCombatState is null)
        {
            return;
        }
        //List<CardModel> drawPile = [.. Owner.PlayerCombatState.DrawPile.Cards];
        //List<CardModel> handPile = [.. Owner.PlayerCombatState.Hand.Cards];
        if (!IsUpgraded)
        {
            CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToBump, 1, 1);
            IEnumerable<CardModel> results = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner), Owner, cardSelectorPrefs);
            foreach (CardModel card in results)
            {
                await card.BumpAsync(choiceContext);
            }
        }
        else
        {
            CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToBump, 1, 1);
            IEnumerable<CardModel> results = await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this);
            foreach (CardModel card in results)
            {
                await card.BumpAsync(choiceContext);
            }
        }

        if (!IsUpgraded)
        {
            CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToNudge, 1, 1);
            IEnumerable<CardModel> results = await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this);
            foreach (CardModel card in results)
            {
                await NudgeCmd.NudgeAsync(card, NudgeDuration.UntilPlayed);
            }
        } 
        else
        {
            CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToNudge, 1, 1);
            IEnumerable<CardModel> results = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner), Owner, cardSelectorPrefs);
            foreach (CardModel card in results)
            {
                await NudgeCmd.NudgeAsync(card, NudgeDuration.UntilPlayed);
            }
        }
    }
}