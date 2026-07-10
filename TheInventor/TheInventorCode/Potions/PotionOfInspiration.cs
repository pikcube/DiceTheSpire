using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Cards.Token;
using TheInventor.TheInventorCode.Powers;
using TheInventor.TheInventorCode.Utilities;

namespace TheInventor.TheInventorCode.Potions;


public class PotionOfInspiration : TheInventorPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(InventorStaticHoverTips.TemporaryGadget)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        Player player = target?.Player ?? Owner;

        string[] ids = [.. ScrapManager.GetRandomCombatGadgetId(player.RunState.Rng.CombatCardGeneration, 3)];

        GadgetCard[] cards = [GadgetCard1.Create(player), GadgetCard2.Create(player), GadgetCard3.Create(player)];

        for (int n = 0; n < 3; ++n)
        {
            cards[n].SetVars(ScrapManager.AllGadgets[ids[n]], true);
        }

        CardModel? selection = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, player);

        if (selection is null)
        {
            return;
        }

        for (int n = 0; n < 3; ++n)
        {
            cards[n].ResetVars();
        }

        string id = ids[cards.IndexOf(selection)];
        
        TemporaryGadgetPower? power = await TemporaryGadgetPower.ApplyAsync(choiceContext, player.Creature, 1, Owner.Creature, null);

        if (power is null)
        {
            return; //wut?
        }

        await power.SetThisAsync(id);
        await power.LinkedGadgetModel.OnRechargeAsync(choiceContext, Owner);

    }
}