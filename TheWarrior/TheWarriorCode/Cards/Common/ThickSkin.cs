using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Common
{
    public class ThickSkin() : TheWarriorCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new BlockVar(5M, BlockProps.card)];
        public override bool GainsBlock => true;
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {

            if (CombatState is null)
            {
                return;
            }
            await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
            await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
            if (IsUpgraded)
            {
                await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
            }

            await base.OnPlay(choiceContext, cardPlay);
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }
        protected override void OnUpgrade()
        {
            base.OnUpgrade();
            DynamicVars.Cards.UpgradeValueBy(1);
            DynamicVars.Block.UpgradeValueBy(1);
        }
    }
}