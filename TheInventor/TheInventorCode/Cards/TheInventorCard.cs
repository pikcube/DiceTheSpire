using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
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

        protected sealed override IEnumerable<IHoverTip> ExtraHoverTips => [.. GetGadgetHoverTip(), ..ExtraInventorHoverTips];

        protected IEnumerable<IHoverTip> GetGadgetHoverTip()
        {
            AbstractGadget gadget = Gadget.AllGadgets[GetScrapId];
            if (gadget is DefaultGadget)
            {
                return [];
            }
            return [new HoverTip(gadget.Title, gadget.Description, ModelDb.Relic<Gadget>().Icon)];
        }
        public abstract string GetScrapId { get; }

        public virtual Task OnScrapAsync(AbstractGadget linkedGadget)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnSkippedAsync()
        {
            return Task.CompletedTask;
        }

        public virtual bool ModifyScrap(Gadget gadget, AbstractGadget linkedGadget)
        {
            return false;
        }

    }
}