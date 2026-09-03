using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace DiceTheSpire.DiceTheSpireCode.Common.Relics;


public class ClosedToeShoes : TheInventorRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner != player)
        {
            return;
        }

        
        CardModel? card = Owner.PlayerCombatState?.Hand.Cards
            .Where(ModelDb.Enchantment<Swift>().CanEnchant)
            .Where(c => !c.Keywords.Contains(CardKeyword.Unplayable))
            .TakeRandom(1, Owner.RunState.Rng.CombatTargets).SingleOrDefault();

        if (card is null)
        {
            return;
        }

        CardCmd.Enchant<Swift>(card, 1);
        NCardEnchantVfx? child = NCardEnchantVfx.Create(card);
        if (child != null)
        {
            NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(child);
        }
    }

    private static bool Filter(CardModel card)
    {
        return card.CanPlay() && ModelDb.Enchantment<Swift>().CanEnchant(card);
    }
}