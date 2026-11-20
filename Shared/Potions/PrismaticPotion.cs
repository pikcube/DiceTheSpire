using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Potions;

public class PrismaticPotion : TheThiefPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        if (target?.Player is null)
        {
            return;
        }

        CardModel? card = await CardSelectCmd.FromChooseACardScreen(choiceContext,
        [
            .. DiceyHelperFunctions.GetDistinctOfClassForCombat(target.Player, ModelDb.AllCharacterCardPools, 3,
                target.Player.RunState.Rng.CombatCardGeneration)
        ], target.Player);
        if (card is null)
        {
            return;
        }
        card.SetToFreeThisTurn();
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, target.Player);
    }
}