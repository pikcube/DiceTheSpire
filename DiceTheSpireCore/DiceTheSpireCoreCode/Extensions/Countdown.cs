using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;

public static class Countdown
{
    extension(ICountdown card)
    {
        public void ResetCount()
        {
            card.CurrentCount = card.MaxCount;
        }

        public void DecrementCount(int decrementBy = 1)
        {
            card.CurrentCount -= decrementBy;
            if (card.CurrentCount < 0)
            {
                card.CurrentCount = 0;
            }
        }

        public void UpgradeCountdown(int upgradeBy)
        {
            card.MaxCount += upgradeBy;
        }
    }
}