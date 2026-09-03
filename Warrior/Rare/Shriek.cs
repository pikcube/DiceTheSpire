using BaseLib.Extensions;
using DiceTheSpire.Common.DynamicVars;
using DiceTheSpire.Common.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Warrior.Rare;
public class Shriek() : TheWarriorCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WarriorShriekPower>(1M), .. RangeVars.Make(0, 2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WarriorShriekPower>(DynamicVars.Power<WarriorShriekPower>().IntValue)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<WarriorShriekPower>(choiceContext, Owner.Creature, DynamicVars.Power<WarriorShriekPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<WarriorShriekPower>().UpgradeValueBy(1);
    }
}

