using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Thief.Rare;

public class BearCharge() : TheThiefCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self), ICountdown, IFuryModifier
{
    public int BaseCount { get; set; } = 3;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FuryPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FuryPower>()];
    public bool ShouldIgnoreFury => true;
    public bool ShouldMaintainFury => false;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<FuryPower>(choiceContext, Owner.Creature, DynamicVars["FuryPower"].BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.UpgradeCountdownBy(-1);
    }
}