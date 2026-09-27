using BaseLib.Abstracts;
using BaseLib.Extensions;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Cards;

public abstract class DiceTheSpireCard(int cost, CardType type, CardRarity rarity, TargetType target) :
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

    public virtual Texture2D GetPips(int? cost, bool isPretend, CardCostColor? energyCostColor = null) =>
        PipCard.GetPipsForMod(this, MainFile.ResPath, cost, isPretend, energyCostColor);

    protected virtual bool IsDiceyPlayable => true;

    protected sealed override bool IsPlayable => IsCountdownPlayable() && IsDiceyPlayable;

    private bool IsCountdownPlayable()
    {
        if (this is not ICountdown countdown)
        {
            return true;
        }

        CardPile? hand = Owner.PlayerCombatState?.Hand;
        if (hand is null)
        {
            return true;
        }

        return hand.Cards.Count(c => c != this) >= countdown.MaxCount;
    }

    public async Task<(int, int)> DiceySpendResources(PlayerChoiceContext choiceContext)
    {
        (int, int) spent = await SpendResources();
        if (this is ICountdown countdown)
        {
            //todo: Pay cost
        }
        return spent;
    }
}