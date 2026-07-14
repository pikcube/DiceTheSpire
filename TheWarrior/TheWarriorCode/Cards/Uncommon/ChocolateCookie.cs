using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheWarrior.TheWarriorCode.Extensions;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon;
    public class ChocolateCookie() : TheWarriorCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self), IRangeCard
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FuryPower>(1M)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [BetterStaticHoverTips.RangeHoverTip(this), HoverTipFactory.FromPower<FuryPower>(DynamicVars.Power<FuryPower>().IntValue)];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
             await PowerCmd.Apply<FuryPower>(choiceContext, Owner.Creature, DynamicVars.Power<FuryPower>().IntValue, Owner.Creature, this);
        }
        public int MinimumCost => 3;
        public int MaximumCost => 3;

        protected override void OnUpgrade()
        {
            DynamicVars.Power<FuryPower>().UpgradeValueBy(1);
        }
}

