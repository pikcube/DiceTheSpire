using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Powers;

public class FuryPower : DiceTheSpirePower
{
    private List<CardModel> _cardsThatShouldRemoveFury = [];
    private List<CardModel> _cardsThatNeedVigorRemoved = [];
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount,
        Creature? applier,
        out decimal modifiedAmount)
    {
        if (canonicalPower is not VigorPower vigorPower || target != Owner || amount >= 0 || _cardsThatShouldRemoveFury.Count == 0)
        {
            modifiedAmount = amount;
            return false;
        }

        vigorPower.PrivateFieldWrapper<PowerModel, object>("_internalData").Value =
            AccessTools.DeclaredMethod(typeof(VigorPower), "InitInternalData").Invoke(vigorPower, []);

        _cardsThatNeedVigorRemoved.AddRange(_cardsThatShouldRemoveFury);
        modifiedAmount = 0;
        return true;
    }

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        //return card.Owner.Creature == this.Owner ? playCount: playCount + 1;
        //|| CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>((Func<CardPlayStartedEntry, bool>)(e => e.Actor == this.Owner && e.HappenedThisTurn(this.CombatState))) >= this.Amount
        if (card.Owner.Creature != Owner)
        {
            return playCount;
        }

        if (card is IFuryModifier { ShouldIgnoreFury: true })
        {
            return playCount;
        }

        int furyCount = Amount; //1;

        DiceyHooks.ModifyFuryPlayCount(card.Owner.RunState, this, card, ref furyCount);

        return playCount + furyCount;

    }

    public override async Task AfterModifyingCardPlayCount(CardModel card)
    {
        Flash();
        if (card is IFuryModifier { ShouldMaintainFury: true })
        {
            return;
        }
        //await PowerCmd.Decrement(this);
        _cardsThatShouldRemoveFury.Add(card);
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (_cardsThatShouldRemoveFury.Contains(cardPlay.Card) && cardPlay.IsLastInSeries)
        {
            _cardsThatShouldRemoveFury.Remove(cardPlay.Card);
            await PowerCmd.Remove(this);
        }

        if (_cardsThatNeedVigorRemoved.Contains(cardPlay.Card) && cardPlay.IsLastInSeries)
        {
            _cardsThatNeedVigorRemoved.RemoveAll(c => c == cardPlay.Card);
            await PowerCmd.Remove<VigorPower>(Owner);
        }
    }

    protected override void AfterCloned()
    {
        base.AfterCloned();
        _cardsThatShouldRemoveFury = [];
        _cardsThatNeedVigorRemoved = [];
    }
}