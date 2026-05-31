using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheThief.TheThiefCode.Cards.Uncommon;

public class Blender() : TheThiefCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromCard<Pip>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.PlayerCombatState is null)
        {
            return;
        }
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
        int cost = 0;
        if (card is not null)
        {
            if (card.EnergyCost.CostsX)
            {
                cost = Owner.PlayerCombatState.Energy;
            }
            else
            {
                cost = card.EnergyCost.Canonical;
            }

            if (IsUpgraded)
            {
                cost += 1;
            }
        }

        List<Pip> list = new List<Pip>();
        for (int i = 0; i < cost; i++)
        {
            list.Add(new Pip());
        }

        await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, Owner);
    }
}