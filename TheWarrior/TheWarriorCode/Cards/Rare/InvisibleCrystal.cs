using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.DynamicVars;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;


namespace TheWarrior.TheWarriorCode.Cards.Rare
{

    public class InvisibleCrystal() : TheWarriorCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<IntangiblePower>(2M), .. RangeVars.Make(2, 3)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<IntangiblePower>(DynamicVars.Power<IntangiblePower>().IntValue)];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<IntangiblePower>(choiceContext, Owner.Creature, DynamicVars.Power<IntangiblePower>().IntValue, Owner.Creature, this);
        }
        protected override void OnUpgrade()
        {
            DynamicVars.MaxRange.UpgradeValueBy(-1);
            DynamicVars.MinRange.UpgradeValueBy(-1);
        }
    }
}
