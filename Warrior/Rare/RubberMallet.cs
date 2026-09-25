using DiceTheSpire.Inventor.Rare;
using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Listeners;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Rare;
public class RubberMallet() : TheWarriorCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy), IAfterRerollListener
{
    public int RerolledThisCombat { get; set; }
    protected override IEnumerable<DynamicVar> CanonicalVars => [.. MakeCalculatedDamage(6, Bonus, 2)]; 
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];

    private static decimal Bonus(CardModel arg1, Creature? arg2)
    {
        return arg1 is not RubberMallet rm ? 1 : rm.RerolledThisCombat;
    }

    public override Task BeforeCombatStart()
    {
        RerolledThisCombat = 0;
        return Task.CompletedTask;
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

    }

    public Task AfterRerollAsync(PlayerChoiceContext choiceContext, CardModel card, bool isFixed, int originalCost, int getAmountToSpend, RerollDuration duration)
    {
        RerolledThisCombat++;
        return Task.CompletedTask;
    }
    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(3);
        DynamicVars.ExtraDamage.UpgradeValueBy(1);
    }
}