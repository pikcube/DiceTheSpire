using BaseLib.Extensions;
using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Thief.Uncommon;

public class Fortify() : TheThiefCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self), ICountdown
{
    public int BaseCount { get; set; } = 2;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PlatingPower>(5)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PlatingPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, DynamicVars.Power<PlatingPower>().BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<PlatingPower>().UpgradeValueBy(3);
    }
}