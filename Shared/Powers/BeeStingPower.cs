using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Utility;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;

[UsedImplicitly]
public class BeeStingPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner == player.Creature)
        {
            CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToShock, Amount, Amount);
            IEnumerable<CardModel> results = await CardSelectCmd.FromHand(choiceContext, player, cardSelectorPrefs, null, this);
            await Task.WhenAll(results.Select(card => card.ShockAsync(choiceContext)));
        }
    }
}