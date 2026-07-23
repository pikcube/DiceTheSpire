using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon
{

    public class Cantrips() : TheWarriorCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CantripsPower>(3M)];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<CantripsPower>(choiceContext, Owner.Creature, DynamicVars.Power<CantripsPower>().IntValue, Owner.Creature, this);
        }
        protected override void OnUpgrade()
        {
            DynamicVars.Power<CantripsPower>().UpgradeValueBy(2);
        }
    }
}
