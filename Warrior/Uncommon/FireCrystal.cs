using BaseLib.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Warrior.Uncommon;
public class FireCrystal() : TheWarriorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self), ICrystalCard, IFuryModifier
{
    public bool ShouldIgnoreFury => true;
    public bool ShouldMaintainFury => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(6), new PowerVar<FuryPower>(2M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FuryPower>(DynamicVars.Power<FuryPower>().IntValue)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.PlayerCombatState is null)
        {
            return;
        }
        if (IsPlayable)
        {
            await PowerCmd.Apply<FuryPower>(choiceContext, Owner.Creature, DynamicVars.Power<FuryPower>().IntValue, Owner.Creature, this);
        }
    }
    protected override bool IsDiceyPlayable => Owner.PlayerCombatState?.Hand.Cards.Sum(c => c.EnergyCost.GetAmountToSpend()) + Owner.PlayerCombatState?.PlayPile.Cards.Sum(c => c.EnergyCost.GetAmountToSpend()) >= DynamicVars.Energy.IntValue;

    protected override bool ShouldGlowGoldInternal => IsPlayable;
    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(-1);
    }

}
