using BaseLib.Extensions;
using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using DiceTheSpire.Shared.Extensions;

namespace DiceTheSpire.Warrior.Rare;
public class UnstoppableForm() : TheWarriorCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<UnstoppableFormPower>(2M), .. RangeVars.Make(3, 3)];
    //protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Dodge)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<UnstoppableFormPower>(choiceContext, Owner.Creature, DynamicVars.Power<UnstoppableFormPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        //DynamicVars.Power<UnstoppableFormPower>().UpgradeValueBy(1);
        DynamicVars.MinRange.UpgradeValueBy(-3);
    }
}