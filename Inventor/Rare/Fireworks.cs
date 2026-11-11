using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Listeners;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Rare;

public class Fireworks() : TheInventorCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy), ISimpleCalculatedDamageCard
{
    public int BaseDamage => 0;
    public int ExtraDamageMultiplier => 3;
    public ValueProp DamageType => DamageProps.card;

    public decimal CalculateDamage(Creature? target)
    {
        if (CombatState is null)
        {
            return 1;
        }

        return CombatManager.Instance.History.Entries.OfType<PowerReceivedEntry>()
            .Count(pre => DiceyHelperFunctions.IsDebuffAtAmount(pre.Power, pre.Amount) && pre.Applier == Owner.Creature);
    }
    public override string GetScrapId => nameof(Burrower);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(2);
    }
}