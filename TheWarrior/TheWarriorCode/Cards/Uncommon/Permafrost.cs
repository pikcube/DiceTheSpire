using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon
{


    public class PermafrostRetention() : TheWarriorCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PermafrostRetentionPower>(3M)];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<PermafrostRetentionPower>(choiceContext, Owner.Creature, DynamicVars.Power<PermafrostRetentionPower>().IntValue, Owner.Creature, this);
        }
        protected override void OnUpgrade()
        {
            DynamicVars.Power<PermafrostRetentionPower>().UpgradeValueBy(1);
        }
    }
}
