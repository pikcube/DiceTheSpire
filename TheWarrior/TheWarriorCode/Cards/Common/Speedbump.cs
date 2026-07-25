
using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
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
public class Speedbump() : TheWarriorCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)//, IRangeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Bump)]; //BetterStaticHoverTips.RangeHoverTip(this),
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        var cards = await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        foreach (CardModel card in cards)
        {
            await card.BumpAsync(choiceContext);
            if(IsUpgraded)
            {
                if(card.EnergyCost.CostsX==false)
                {
                    card.EnergyCost.SetThisTurnOrUntilPlayed(card.EnergyCost.GetAmountToSpend() - 1);
                }
            }
        }
        
        return;
    }

    //public int MinimumCost => 0;
    //public int MaximumCost => 0;
    protected override void OnUpgrade()
    {
        base.OnUpgrade();
    }
}




