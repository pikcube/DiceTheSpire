using DiceTheSpireCore.DiceTheSpireCoreCode;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TheWarrior.TheWarriorCode.Cards.Uncommon
{

    public class NudgeBlade() : TheWarriorCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, DamageProps.card)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Nudge)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner.PlayerCombatState is null || CombatState is null || cardPlay.Target is null)
            {
                return;
            }

            foreach (CardModel card in Owner.PlayerCombatState.Hand.Cards.ToArray())
            {
                if (card.CurrentUpgradeLevel > 0 || IsUpgraded)
                {
                    await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                        .FromCard(this, cardPlay)
                        .Targeting(cardPlay.Target)
                        .WithHitFx(VfxCmd.slashPath)
                        .Execute(choiceContext);
                }
                await card.NudgeAsync(choiceContext);
            }
        }
    }
}
