using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheWarrior.TheWarriorCode.Cards.Common
{

    public class SineWave() : TheWarriorCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Bump>(), HoverTipFactory.FromCard<Nudge>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (CombatState is null)
            {
                return;
            }
            await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Bump>(Owner), PileType.Hand, Owner);
            await Cmd.Wait(0.25f);
            await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Nudge>(Owner), PileType.Hand, Owner);
            await Cmd.Wait(0.25f);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }

    }

}
