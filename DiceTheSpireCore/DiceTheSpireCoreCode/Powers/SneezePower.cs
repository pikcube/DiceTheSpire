using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Utility;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;

public class SneezePower : DiceTheSpireCorePower, IOnBlinkListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public Task AfterCardBlinkedAsync(PlayerChoiceContext choiceContext, CardModel card)
    {
        return CreatureCmd.GainBlock(Owner, Amount, BlockProps.nonCardUnpowered, null);
    }
}