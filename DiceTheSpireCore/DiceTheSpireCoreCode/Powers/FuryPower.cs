using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
public class FuryPower : DiceTheSpireCorePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    private List<CardModel> CardsModifiedByFury { get; set; } = [];

    private bool _shouldIgnoreNextRemoval;

    protected override void AfterCloned()
    {
        //Anytime you override AfterCloned, call the base method or bad things happen
        base.AfterCloned();
        //Since a List is a reference type, we need to insure that we get a brand
        //new empty list instead of a reference to the canonical model's list
        CardsModifiedByFury = [];
    }

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        //return card.Owner.Creature == this.Owner ? playCount: playCount + 1;
        //|| CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>((Func<CardPlayStartedEntry, bool>)(e => e.Actor == this.Owner && e.HappenedThisTurn(this.CombatState))) >= this.Amount
        return card.Owner.Creature == Owner ? playCount + 1 : playCount;
    }

    public override Task AfterModifyingCardPlayCount(CardModel card)
    {
        Flash();
        //This runs before the card is played, but we can't remove the power until after the card play
        //resolves due to Flaming Sword, so we defer that call to AfterCardPlayed
        CardsModifiedByFury.Add(card);
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!cardPlay.IsLastInSeries)
        {
            return;
        }

        //The remove function return false if the item wasn't in the list, and we don't want to decrement unless the card was modified by Fury
        if (!CardsModifiedByFury.Remove(cardPlay.Card))
        {
            return;
        }

        if (_shouldIgnoreNextRemoval)
        {
            _shouldIgnoreNextRemoval = false;
        }
        else
        {
            await PowerCmd.Decrement(this);
        }
    }

    public void IgnoreNextRemoval() => _shouldIgnoreNextRemoval = true;
}
