using DiceTheSpire.Shared.Cards;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Potions;
public class DiceyDrink : TheWarriorPotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<CombatRoll>(), HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        while (target?.Player is null)
        {
            target = Owner.Creature;
        }

        if (Owner.Creature.CombatState is null)
        {
            return;
        }


        for (int n = 0; n < 3; ++n)
        {
            CombatRoll card = Owner.Creature.CombatState.CreateCard<CombatRoll>(Owner);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }

    }
}