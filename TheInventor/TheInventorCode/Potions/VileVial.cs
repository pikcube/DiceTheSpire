using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInventor.TheInventorCode.Utilities;

namespace TheInventor.TheInventorCode.Potions;

public class VileVial : TheInventorPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyEnemy;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("DebuffCount", 5)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        if (target is null)
        {
            return;
        }
        for (int n = 0; n < DynamicVars["DebuffCount"].IntValue; ++n)
        {
            await InventorHelperFunctions.ApplyRandomDebuffAsync(choiceContext, Owner.RunState, target, Owner.Creature, null);
        }
    }
}