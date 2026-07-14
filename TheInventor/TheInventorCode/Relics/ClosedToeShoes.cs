using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace TheInventor.TheInventorCode.Relics;


public class ClosedToeShoes : TheInventorRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner != player || Owner.PlayerCombatState?.TurnNumber != 1)
        {
            return;
        }

        CardModel? card = Owner.PlayerCombatState.Hand.Cards
            .Where(c => ModelDb.Enchantment<Swift>().CanEnchant(c) && c.CanPlay())
            .TakeRandom(1, Owner.RunState.Rng.CombatCardSelection)
            .FirstOrDefault();

        card ??= Owner.PlayerCombatState.DrawPile.Cards.FirstOrDefault(c => ModelDb.Enchantment<Swift>().CanEnchant(c) && c.CanPlay());

        if (card is null)
        {
            return;
        }

        CardCmd.Enchant<Swift>(card, 3);
        NCardEnchantVfx? child = NCardEnchantVfx.Create(card);
        if (child != null)
        {
            NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(child);
        }
    }
}