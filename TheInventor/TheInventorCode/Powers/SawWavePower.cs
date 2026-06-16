using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace TheInventor.TheInventorCode.Powers;

[UsedImplicitly]
public class SawWavePower : TheInventorPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (applier != Owner || power.Owner != Owner || power.Type != PowerType.Debuff)
        {
            return;
        }

        await DexterityPower.ApplyAsync(choiceContext, Owner, Amount, Owner, cardSource);
    }
}