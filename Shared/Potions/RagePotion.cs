using BaseLib.Extensions;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
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