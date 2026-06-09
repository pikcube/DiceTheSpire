using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon
{

public class TargetShield() : TheWarriorCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DexterityPower>(0M)];

        //protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        //    [HoverTipFactory.FromPower(DexterityPower)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            TargetShield cardSource = this;
            await PowerCmd.Apply<targetShieldPower>(choiceContext, Owner.Creature, (1M),
            Owner.Creature, this);
            DexterityPower dexterityPower = await PowerCmd.Apply<DexterityPower>(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars.Dexterity.BaseValue, cardSource.Owner.Creature, (CardModel)cardSource);
            //await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, DynamicVars.Dexterity, Owner.Creature, this);
            return;
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Dexterity.UpgradeValueBy(1);
        }
    }

}



