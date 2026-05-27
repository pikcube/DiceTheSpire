using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Commands;
using Pikcube.Common.Powers;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Uncommon;


public class Uncle() : TheInventorCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
{
    public override string GetScrapId => nameof(Crack);

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new IntVar("Delay", 4), new DamageVar(60, DamageProps.nonCardHpLoss)];
    public LocString JinxDescription => new("cards", Id.Entry + ".jinxDescription");

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return Task.CompletedTask;
        }

        return JinxCmd.JinxAsync(choiceContext, CombatState.Enemies, DynamicVars["Delay"].IntValue, false,
            JinxDescription, JinxComplete, Owner.Creature, this);
    }
    private Task JinxComplete(PlayerChoiceContext choiceContext, Creature target)
    {
        return CreatureCmd.Damage(choiceContext, target, DynamicVars.Damage, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(15);
    }
}