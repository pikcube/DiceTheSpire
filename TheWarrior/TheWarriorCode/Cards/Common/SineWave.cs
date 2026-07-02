using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using static Godot.HttpRequest;

namespace TheWarrior.TheWarriorCode.Cards.Common
{

    public class SineWave() : TheWarriorCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (CombatState is null || Owner is null || Owner.PlayerCombatState is null)
            {
                return;
            }
            //List<CardModel> drawPile = [.. Owner.PlayerCombatState.DrawPile.Cards];
            //List<CardModel> handPile = [.. Owner.PlayerCombatState.Hand.Cards];
            if (!IsUpgraded)
                {
                    CardSelectorPrefs cardSelectorPrefs = new(new LocString("card_selection", "TO_BUMP"), 1, 1);
                    IEnumerable<CardModel> results = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner), Owner, cardSelectorPrefs);
                    foreach (CardModel card in results)
                    {
                        await card.BumpAsync(choiceContext);
                    }
                } else
                {
                    CardSelectorPrefs cardSelectorPrefs = new(new LocString("card_selection", "TO_BUMP"), 1, 1);
                    IEnumerable<CardModel> results = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Hand.GetPile(Owner), Owner, cardSelectorPrefs);
                    foreach (CardModel card in results)
                    {
                        await card.BumpAsync(choiceContext);
                    }
                }

                if (!IsUpgraded)
                {
                    CardSelectorPrefs cardSelectorPrefs = new(new LocString("card_selection", "TO_NUDGE"), 1, 1);
                    IEnumerable<CardModel> results = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Hand.GetPile(Owner), Owner, cardSelectorPrefs);
                    foreach (CardModel card in results)
                    {
                        await card.NudgeAsync(choiceContext);
                    }
                } else
                {
                    CardSelectorPrefs cardSelectorPrefs = new(new LocString("card_selection", "TO_NUDGE"), 1, 1);
                    IEnumerable<CardModel> results = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner), Owner, cardSelectorPrefs);
                    foreach (CardModel card in results)
                    {
                        await card.NudgeAsync(choiceContext);
                    }
                }
        }

        protected override void OnUpgrade()
        {
            base.OnUpgrade();
        }

    }

}
