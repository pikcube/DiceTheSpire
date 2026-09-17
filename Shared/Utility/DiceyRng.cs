using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Rngs;

namespace DiceTheSpire.Shared.Utility;

public static class DiceyRng
{
    public static IEnumerable<RunRngType> All => 
    [
        CombatPowerGeneration
    ];

    [CustomEnum]
    public static RunRngType CombatPowerGeneration = 0;
}