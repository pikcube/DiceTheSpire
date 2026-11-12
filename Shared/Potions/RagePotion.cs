using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Pikcube.Common.Code.Extensions;

namespace DiceTheSpire.Shared.Potions;
public class RagePotion : TheWarriorPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;
    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FuryPower>()];
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        target ??= Owner.Creature;
        await FuryPower.ApplyAsync(choiceContext, target, Owner.Creature.GetPowerAmount<FuryPower>(), Owner.Creature, null);

    }
}