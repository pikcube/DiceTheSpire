using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWarrior.TheWarriorCode.Cards;

namespace TheWarrior.TheWarriorCode.Cards.Common;


public class Crunch() : TheWarriorCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new DamageVar(7M, DamageProps.card)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Bump)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        if (Owner is null || CombatState is null)
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
       .FromCard(this, cardPlay)
       .Targeting(cardPlay.Target)
       .WithHitFx(VfxCmd.slashPath)
       .Execute(choiceContext);
        
        if (IsUpgraded)
        {
            CardSelectorPrefs cardSelectorPrefs = new(new LocString("card_selection", "TO_BUMP"), 0, DynamicVars.Cards.IntValue);
            CardModel[] cardChoices = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
            foreach (CardModel card in cardChoices)
            {
                await card.BumpAsync(choiceContext);
            }
        }
        else
        {
            CardModel[] cards =
            [
           ..PileType.Hand.GetPile(Owner).Cards
                        .Where(c => !c.Keywords.Contains(CardKeyword.Unplayable) && (c.IsUpgradable == true || c.IsUpgraded == true))
                        .TakeRandom((int)DynamicVars.Cards.BaseValue, Owner.RunState.Rng.CombatCardSelection)
            ];
            foreach (CardModel card in cards)
            {
                await card.BumpAsync(choiceContext);
            }
        }

        return;
    }

    protected override void OnUpgrade()
    {
        base.OnUpgrade();
    }
}


