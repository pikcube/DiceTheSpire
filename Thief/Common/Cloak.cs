using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Thief.Common;

public class Cloak() : TheThiefCard(0, CardType.Skill, CardRarity.Common, TargetType.Self), ICountdown
{
    public int MaxCount
    {
        get;
        set;
    } = 2;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ReducePower>(3M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ReducePower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<CloakPower>(choiceContext, Owner.Creature, DynamicVars["ReducePower"].EnchantedValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ReducePower"].UpgradeValueBy(2);
    }
}