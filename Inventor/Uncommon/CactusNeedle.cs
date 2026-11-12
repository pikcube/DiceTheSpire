using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Code.Extensions;

namespace DiceTheSpire.Inventor.Uncommon;

public class CactusNeedle() : TheInventorCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(4, DamageProps.cardUnpowered), new PowerVar<ThornsPower>(1)];
    public override string GetScrapId => nameof(Needle);

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips =>
        [.. HoverTipFactory.FromPowerWithPowerHoverTips<ThornsPower>()];

    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        if (CombatState is null)
        {
            //wut?
            return;
        }
        IEnumerable<DamageResult> results = await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, DynamicVars.Damage.BaseValue, DynamicVars.Damage.Props, Owner.Creature, this, null);
        foreach (DamageResult _ in results.Where(r => r.TotalDamage > 0))
        {
            await ThornsPower.ApplyAsync(choiceContext, Owner, 1, Owner, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}