using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpire.Shared.Extensions;


public static class CountdownExtensions
{
    extension(ICountdown card)
    {
        public void UpgradeCountdownBy(int addend)
        {
            card.BaseCount += addend;
        }

        public int Count
        {
            get
            {
                return card.BaseCount;
            }
        }
    }
}