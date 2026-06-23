using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarrior.TheWarriorCode.Cards.Rare
{
    public class CrescentMoonBlade() : TheWarriorCard(-1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(9, BlockProps.card), new DamageVar(12, DamageProps.card)];
        public override bool GainsBlock => true;
        protected override bool HasEnergyCostX => true;
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (CombatState is null)
            {
                return;
            }

            int xValue = cardPlay.Card.ResolveEnergyXValue();

            if (CombatState?.RoundNumber % 2 == 0 != IsUpgraded)
            {
                for (int n = 0; n < xValue; ++n)
                {
                    await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this)
                    .Targeting(cardPlay.Target)
                    .WithHitFx(VfxCmd.slashPath)
                    .Execute(choiceContext);
                }
            }
            else
            {
                for (int n = 0; n < xValue; ++n)
                {
                    await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(2);
            DynamicVars.Damage.UpgradeValueBy(4);
        }
    }   
}
