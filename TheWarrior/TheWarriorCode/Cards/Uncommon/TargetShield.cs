using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon;

public class TargetShield() : TheWarriorCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DexterityPower>(0M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DexterityPower>(DynamicVars.Power<DexterityPower>().IntValue)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        TargetShield cardSource = this;
        await PowerCmd.Apply<TargetShieldPower>(choiceContext, Owner.Creature, 1M,
            Owner.Creature, this);
        await PowerCmd.Apply<DexterityPower>(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars.Dexterity.BaseValue, cardSource.Owner.Creature, cardSource);
        //await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, DynamicVars.Dexterity, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Dexterity.UpgradeValueBy(1);
    }
}