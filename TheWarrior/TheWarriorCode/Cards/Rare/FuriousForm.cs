using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.DynamicVars;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheWarrior.TheWarriorCode.Cards.Rare;
public class FuriousForm() : TheWarriorCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FuriousFormPower>(1), new PowerVar<FuryPower>(1M), .. RangeVars.Make(3, 3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FuriousFormPower>(DynamicVars.Power<FuriousFormPower>().IntValue)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FuriousFormPower>(choiceContext, Owner.Creature, DynamicVars.Power<FuriousFormPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.MinRange.UpgradeValueBy(-3);
    }
}