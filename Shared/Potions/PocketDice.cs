using DiceTheSpire.Shared.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Shared.Potions;

public class PocketDice : TheThiefPotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Pip>()];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        if (target?.Player is null || target.CombatState is null)
        {
            return;
        }

        await Pip.CreateInHandAsync(target.Player, DynamicVars.Cards.IntValue, target.CombatState);
    }
}