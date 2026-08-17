using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheWarrior.TheWarriorCode.Cards.Common;

public class KnuckleDuster() : TheWarriorCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(4), new PowerVar<VigorPower>(4M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Rummage), HoverTipFactory.FromPower<VigorPower>(DynamicVars.Power<VigorPower>().IntValue)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars.Power<VigorPower>().IntValue, Owner.Creature, this);

        CardSelectorPrefs cardSelectorPrefs = new(CardSelectorPrefs.DiscardSelectionPrompt, 0, DynamicVars.Cards.IntValue);
        CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
        foreach (CardModel card in cards)
        {
            await CardCmd.Discard(choiceContext, card);
        }

        if (cards.Length == 0)
        {
            return;
        }

        await CardPileCmd.Draw(choiceContext, cards.Length, Owner);

    }

    protected override void OnUpgrade() => AddKeyword(CardKeyword.Retain);

}
