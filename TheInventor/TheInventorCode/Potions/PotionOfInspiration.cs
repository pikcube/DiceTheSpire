using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheInventor.TheInventorCode.Utilities;

namespace TheInventor.TheInventorCode.Potions;


public class PotionOfInspiration : TheInventorPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(InventorStaticHoverTips.TemporaryGadget)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        Player player = target?.Player ?? Owner;

        await ScrapManager.RandomizeAllGadgetsAsync(choiceContext, Owner, null);
    }
}