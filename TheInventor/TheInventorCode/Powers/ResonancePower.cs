using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;
using Pikcube.Common.Powers;

namespace TheInventor.TheInventorCode.Powers;


public class ResonancePower : TheInventorPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (applier != Owner || power.Owner != Owner || power.Type != PowerType.Debuff)
        {
            return;
        }

        foreach (Creature target in CombatState.Enemies)
        {
            PowerModel p = GetPowerToApply(power, ref amount);
            if (amount != 0)
            {
                await PowerCmd.Apply(choiceContext, p, target, amount, Owner, null);
            }
        }
    }

    private static PowerModel GetPowerToApply(PowerModel power, ref decimal amount)
    {
        switch (power)
        {
            case TheGambitPower:
                amount = 99;
                return ModelDb.Power<VulnerablePower>();
            case BackfirePower:
                amount = 99;
                return ModelDb.Power<WeakPower>();
            case ConfusedPower:
                amount = -3;
                return ModelDb.Power<StrengthPower>().StrongMutableClone();
            case BorrowedTimePower:
            case ShockPower:
                amount = 2;
                return ModelDb.Power<VulnerablePower>().StrongMutableClone();
            case NoEnergyGainPower:
            case NoDrawPower:
            case BiasedCognitionPower:
            case MegaCrit.Sts2.Core.Models.Powers.FocusPower:
                amount = 2;
                return ModelDb.Power<WeakPower>().StrongMutableClone();
            case NoBlockPower:
            case FrailPower:
                amount = 1;
                return ModelDb.Power<FreezePower>().StrongMutableClone();
            case CursedPower:
                amount *= 3;
                return ModelDb.Power<DoomPower>().StrongMutableClone();
            case DemisePower:
            case PoisonPower:
            case NeurosurgePower:
            case StrengthPower:
            case WraithFormPower:
            case DexterityPower:
            case DoomPower:
            case VulnerablePower:
            case WeakPower:
            case ExhaustionPower:
            case FreezePower:
            case LastStandPower:
                return ModelDb.GetById<PowerModel>(power.Id).StrongMutableClone();
            default:
                amount = 1;
                return (power.Id.ToString().GetHashCode() % 3) switch
                {
                    0 => ModelDb.Power<VulnerablePower>().StrongMutableClone(),
                    1 => ModelDb.Power<WeakPower>().StrongMutableClone(),
                    _ => ModelDb.Power<FreezePower>().StrongMutableClone()
                };
        }
    }
}