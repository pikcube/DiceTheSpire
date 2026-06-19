using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers.Models;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheInventor.TheInventorCode.Character;
using TheInventor.TheInventorCode.Extensions;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Relics;

namespace TheInventor.TheInventorCode.Cards
{
    [Pool(typeof(TheInventorCardPool))]
    public abstract class TheInventorCard(int cost, CardType type, CardRarity rarity, TargetType target) :
        CustomCardModel(cost, type, rarity, target)
    {
        //Image size:
        //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
        //Full art: 606x852
        public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

        //Smaller variants of card images for efficiency:
        //Smaller variant of fullart: 250x350
        //Smaller variant of normalart: 250x190

        //Uses card_portraits/card_name.png as image path. These should be smaller images.
        public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
        public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

        protected virtual IEnumerable<IHoverTip> ExtraInventorHoverTips => [];

        protected sealed override IEnumerable<IHoverTip> ExtraHoverTips => [.. GetGadgetHoverTip() , .. GetHeldHoverTip(), ..ExtraInventorHoverTips];

        private IEnumerable<IHoverTip> GetHeldHoverTip()
        {
            if (HasTurnEndInHandEffect)
            {
                yield return HoverTipFactory.Static(InventorStaticHoverTips.Held);
            }
        }

        protected IEnumerable<IHoverTip> GetGadgetHoverTip()
        {
            if (!InventorDebugConfig.ShowGadgetTips)
            {
                yield break;
            }

            GadgetModel gadgetModel = Gadget.AllGadgets[GetScrapId];
            if (gadgetModel is DefaultGadget)
            {
                yield break;
            }

            yield return new HoverTip(gadgetModel.Title, gadgetModel.Description, ModelDb.Relic<Gadget>().Icon);
        }
        public abstract string GetScrapId { get; }

        public virtual Task OnScrapAsync(GadgetModel linkedGadgetModel)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnSkippedAsync()
        {
            return Task.CompletedTask;
        }

        public virtual bool ModifyScrap(Gadget gadget, GadgetModel linkedGadgetModel)
        {
            return false;
        }

        public Texture2D GetPips(int cost, bool isPretend, CardCostColor? energyCostColor = null)
        {
            string costText = cost is < 1 or > 9 ? "0" : $"{cost}";
            if (EnergyCost is { CostsX: false, WasJustUpgraded: true })
            {
                return ResourceLoader.Load<Texture2D>($"charui/Energy/Green/ui_dice_dice{costText}.png".ImagePath());
            }

            energyCostColor ??= CardCostHelper.GetEnergyCostColor(this, CombatState);
            switch (energyCostColor)
            {
                case CardCostColor.Unmodified:
                    return ResourceLoader.Load<Texture2D>($"charui/Energy/ui_dice_dice{costText}.png".ImagePath());
                case CardCostColor.Increased:
                    return ResourceLoader.Load<Texture2D>($"charui/Energy/Blue/ui_dice_dice{costText}.png".ImagePath());
                case CardCostColor.Decreased:
                    return ResourceLoader.Load<Texture2D>($"charui/Energy/Green/ui_dice_dice{costText}.png".ImagePath());
                case CardCostColor.InsufficientResources when !isPretend:
                    return ResourceLoader.Load<Texture2D>($"charui/Energy/Red/ui_dice_dice{costText}.png".ImagePath());
                default:
                    goto case CardCostColor.Unmodified;
            }
        }
    }
}