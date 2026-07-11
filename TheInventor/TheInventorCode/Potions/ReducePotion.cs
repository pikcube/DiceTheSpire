using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Pikcube.Common.Extensions;

namespace TheInventor.TheInventorCode.Potions;

public class ReducePotion : TheInventorPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ReducePower>(2)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        target ??= Owner.Creature;
        await ReducePower.ApplyAsync(choiceContext, target, 2, Owner.Creature, null);
    }
}