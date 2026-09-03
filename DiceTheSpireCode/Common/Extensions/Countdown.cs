using DiceTheSpire.DiceTheSpireCode.Common.Interfaces;
using DiceTheSpire.DiceTheSpireCode.Common.Utility;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.DiceTheSpireCode.Common.Extensions;

//TODO: implement free to play for Countdown cards
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