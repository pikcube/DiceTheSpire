using BaseLib.Abstracts;
using BaseLib.Extensions;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Utility;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Cards;

public abstract class DiceTheSpireCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target), IPipCard
{
    public List<LocalCostModifier> CountdownCostModifiers { get; set; } = [];
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

    protected virtual void AddExtraDiceyArgsToDescription(LocString description)
    {
    }

    protected sealed override void AddExtraArgsToDescription(LocString description)
    {
        if (this is ICountdown countdown)
        {
            description.Add(nameof(countdown.BaseCount), countdown.Count);
        }
        AddExtraDiceyArgsToDescription(description);
    }

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

        return hand.Cards.Count(c => c != this) >= countdown.Count;
    }

    public async Task<(int, int)> DiceySpendResourcesAsync(PlayerChoiceContext choiceContext)
    {
        (int, int) spent = await SpendResources();
        if (this is ICountdown countdown)
        {
            await DoCountdownAsync(choiceContext, countdown.Count);
        }
        return spent;
    }

    private async Task DoCountdownAsync(PlayerChoiceContext choiceContext, int count)
    {
        if (count == 0)
        {
            return;
        }
        //Model registration since our PCC is brand new. Do not remove.
        choiceContext.PushModel(this);
        await CombatManager.Instance.WaitForUnpause();

        //Count down code
        CardSelectorPrefs prefs = new(DiceySelection.ToCountdown, count, count);

        //Okay, so apparently trying to open a card select screen from within Spend Resources causes a use after free bug,
        //but if we move the card we are currently playing to another pile, that doesn't happen
        //I don't know why it doesn't happen, but it works and I'm too tired to fix it
        CardPile? original = Pile;
        CardPile secretPile = new(PileType.Exhaust);
        await CardPileCmd.Add(this, secretPile, skipVisuals: true);


        IEnumerable<CardModel> cards = await CardSelectCmd.FromHand(choiceContext, Owner, prefs, c => c != this, this);

        //Don't forget to put the card back when you are finished with it
        if (original is not null)
        {
            await CardPileCmd.Add(this, original, skipVisuals: true);
        }
        else
        {
            await CardPileCmd.Add(this, PileType.Hand, skipVisuals: true);
        }
        await CardCmd.Discard(choiceContext, cards);

        //Model deregistration since we're all done. Do not remove.
        choiceContext.PopModel(this);
    }

    protected override void AfterCloned()
    {
        base.AfterCloned();
        CountdownCostModifiers = [];
    }

    public CardModel AsCardModel() => this;
}