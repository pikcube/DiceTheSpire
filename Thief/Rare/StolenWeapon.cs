using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Thief.Rare;

public class StolenWeapon() : TheThiefCard(1, CardType.Power, CardRarity.Rare, TargetType.Self), ICountdown
{
    public int BaseCount
    {
        get;
        set;
    } = 2;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StolenWeaponPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<StolenWeaponPower>(choiceContext, Owner.Creature, DynamicVars["StolenWeaponPower"].EnchantedValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.UpgradeCountdownBy(-1);
    }
}