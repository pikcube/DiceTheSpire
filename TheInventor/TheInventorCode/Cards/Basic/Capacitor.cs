using DiceTheSpireCore.DiceTheSpireCoreCode;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Commands;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Basic;

public class Capacitor() : TheInventorCard(-1, CardType.Attack, CardRarity.Basic, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, DamageProps.card), new PowerVar<VulnerablePower>(1)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];

    public override bool HasTurnEndInHandEffect => true;

    public override string GetScrapId => nameof(ShortCircuit);

    public LocString JinxDescription => new("cards", Id.Entry + ".jinxDescription");

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        if (RunState is null || CombatState is null)
        {
            return;
        }

        await CreatureCmd.Damage(choiceContext, CombatState.Enemies, DynamicVars.Damage, Owner.Creature, this);
        await JinxCmd.JinxAsync(choiceContext, CombatState.Enemies, 1, true, JinxDescription, NextTurnAction, Owner.Creature, this);
        await DiceyHooks.OnTurnEndInHand(this, RunState, CombatState);

    }

    private async Task NextTurnAction(PlayerChoiceContext choiceContext, Creature target)
    {
        await PowerCmd.Apply<VulnerablePower>(choiceContext, target, DynamicVars.Vulnerable.IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }
}