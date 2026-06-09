using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Rare
{
    public class Buckler() : TheWarriorCard(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(32, BlockProps.card)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await base.OnPlay(choiceContext, cardPlay);
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }

        protected override void OnUpgrade()
        {
            base.OnUpgrade();
            DynamicVars.Block.UpgradeValueBy(6);
        }

    }
}