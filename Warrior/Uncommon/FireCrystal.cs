using BaseLib.Extensions;
using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Warrior.Uncommon;
public class FireCrystal() : TheWarriorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self), ICrystalCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(6), new PowerVar<VigorPower>(10M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>(DynamicVars.Power<VigorPower>().IntValue)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.PlayerCombatState is null)
        {
            return;
        }
        if (IsPlayable)
        {
            await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars.Power<VigorPower>().IntValue, Owner.Creature, this);
        }
    }
    protected override bool IsDiceyPlayable => Owner.PlayerCombatState?.Hand.Cards.Sum(c => c.EnergyCost.GetAmountToSpend()) + Owner.PlayerCombatState?.PlayPile.Cards.Sum(c => c.EnergyCost.GetAmountToSpend()) >= DynamicVars.Energy.IntValue;

    protected override bool ShouldGlowGoldInternal => IsPlayable;
    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(-1);
    }

}
