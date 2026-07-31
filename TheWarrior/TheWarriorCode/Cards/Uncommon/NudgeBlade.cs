using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon
{

    public class NudgeBlade() : TheWarriorCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3, DamageProps.card)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Nudge)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner.PlayerCombatState is null || CombatState is null || cardPlay.Target is null)
            {
                return;
            }

            int repeats = 0;

            foreach (CardModel card in Owner.PlayerCombatState.Hand.Cards.ToArray())
            {
                if (card.CurrentUpgradeLevel > 0 || IsUpgraded)
                {
                    repeats++;
                }
                await card.NudgeAsync(choiceContext);
            }
            await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
                        .FromCard(this, cardPlay)
                        .Targeting(cardPlay.Target)
                        .WithHitCount(repeats)
                        .WithHitFx(VfxCmd.slashPath)
                        .Execute(choiceContext);
        }
    }
}
