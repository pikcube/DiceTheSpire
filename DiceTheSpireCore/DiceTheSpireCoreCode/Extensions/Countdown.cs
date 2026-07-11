using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;

public static class Countdown
{
    extension(ICountdown card)
    {
        public void ResetCount()
        {
            card.CurrentCount = card.MaxCount;
        }

        public async Task DecrementCountAsync(int decrementBy = 1)
        {
            for (int i = decrementBy; i > 0 && card.CurrentCount >  0; --i)
            {
                --card.CurrentCount;
                await DiceyHooks.OnAfterCardCountsDownAsync((RunState)card.Owner.RunState, card.Owner.Creature.CombatState, (CardModel)card);
            }
        }

        public void UpgradeCountdown(int upgradeBy)
        {
            card.MaxCount += upgradeBy;
        }
    }
}