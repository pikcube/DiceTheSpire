using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon
{

    public class PowerCrystal() : TheWarriorCard(0, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PowerCrystalPower>(1M), new EnergyVar(1)];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<PowerCrystalPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Power<PowerCrystalPower>().UpgradeValueBy(1);
        }
    }
}
