using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Uncommon;


public class SledgeHammer() : TheInventorCard(-1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable, CardKeyword.Ethereal];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(15, DamageProps.cardUnpowered), new PowerVar<VulnerablePower>(2)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];

    public override bool HasTurnEndInHandEffect => true;

    public override string GetScrapId => nameof(ShortCircuit);

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        if (CombatState is null)
        {
            return;
        }

        await CreatureCmd.Damage(choiceContext, CombatState.Enemies, DynamicVars.Damage, Owner.Creature, this, null);
        await NextTurnVulnerablePower.ApplyAsync(choiceContext, CombatState.Enemies, DynamicVars.Vulnerable.IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }
}