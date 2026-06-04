using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace TheThief.TheThiefCode.Cards.Uncommon;

public class Blender() : TheThiefCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromCard<Pip>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.PlayerCombatState is null || CombatState is null)
        {
            return;
        }
        CardSelectorPrefs prefs = new(CardSelectorPrefs.ExhaustSelectionPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
        if (card is null)
        {
            return;
        }

        int cost = card.EnergyCost.CostsX ? Owner.PlayerCombatState.Energy : card.EnergyCost.Canonical;

        if (IsUpgraded)
        {
            cost += 1;
        }
        await card.ExhaustAsync(choiceContext);

        //You wrote a function for this, you should use it
        //List<Pip> list = [];
        //for (int i = 0; i < cost; i++)
        //{
        //    list.Add(new Pip());
        //}

        //await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, Owner);
        await Pip.CreateInHandAsync(Owner, cost, CombatState);
    }
}