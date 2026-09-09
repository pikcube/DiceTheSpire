using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace DiceTheSpire.Shared.Potions;


public class PotionOfInspiration : TheInventorPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(InventorStaticHoverTips.Gadget)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        await ScrapManager.RandomizeAllGadgetsAsync(choiceContext, target?.Player ?? Owner, null);
    }
}