using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers.Models;
using TheThief.TheThiefCode.Character;
using TheThief.TheThiefCode.Extensions;

namespace TheThief.TheThiefCode.Cards;

[Pool(typeof(TheThiefCardPool))]
public abstract class TheThiefCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target), IPipCard
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

    public Texture2D GetPips(int? cost, bool isPretend, CardCostColor? energyCostColor = null)
    {
        string costText = cost switch
        {
            null => "X",
            < 1 or > 9 => "0",
            _ => $"{cost}"
        };

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