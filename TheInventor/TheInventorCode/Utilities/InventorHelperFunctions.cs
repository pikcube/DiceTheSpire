using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;

namespace TheInventor.TheInventorCode.Utilities;

public static class InventorHelperFunctions
{
    public static async Task ApplyRandomDebuffAsync(PlayerChoiceContext choiceContext, IRunState runState, Creature target, Creature? applier, CardModel? cardSource, bool silent = false)
    {
        PowerModel power;
        decimal amount;
        switch (runState.Rng.CombatOrbGeneration.NextInt(9))
        {
            case 8:
                power = ModelDb.Power<DebilitatePower>().StrongMutableClone();
                amount = 1;
                break;
            case 7:
                power = ModelDb.Power<ShrinkPower>().StrongMutableClone();
                amount = 1;
                break;
            case 6:
                power = ModelDb.Power<DarkShacklesPower>().StrongMutableClone();
                amount = 7;
                break;
            case 5:
                power = ModelDb.Power<DoomPower>().StrongMutableClone();
                amount = 6;
                break;
            case 4:
                power = ModelDb.Power<PoisonPower>().StrongMutableClone();
                amount = 4;
                break;
            case 3:
                power = ModelDb.Power<DemisePower>().StrongMutableClone();
                amount = 5;
                break;
            case 2:
                power = ModelDb.Power<FreezePower>().StrongMutableClone();
                amount = 1;
                break;
            case 1:
                power = ModelDb.Power<WeakPower>().StrongMutableClone();
                amount = 2;
                break;
            default:
                power = ModelDb.Power<VulnerablePower>().StrongMutableClone();
                amount = 2;
                break;
        }

        await PowerCmd.Apply(choiceContext, power, target, amount, applier, cardSource, silent);
    }
}