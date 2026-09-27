using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Shared.Extensions;

//TODO: implement free to play for Countdown cards
public static class Countdown
{
    extension(ICountdown card)
    {
        public void UpgradeCountdownBy(int addend)
        {
            card.MaxCount += addend;
        }
    }
}