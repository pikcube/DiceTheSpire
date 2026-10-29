using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Shared.Listeners;

public interface ISimpleCalculatedDamageCard
{
    public int CanonicalCalculatedDamageBase { get; }

    public virtual int CanonicalCalculatedDamageMultiplier => 1;

    public virtual ValueProp DamageType => ValueProp.Move;

    public decimal CalculateDamage(Creature? target);
}

[HarmonyPatch(typeof(CardModel), "CanonicalVars", MethodType.Getter)]
public static class SimpleCalculatedDamagePatch
{
    [HarmonyPostfix]
    public static IEnumerable<DynamicVar> AddCalculatedDamageVars(IEnumerable<DynamicVar> __result,
        CardModel __instance)
    {
        if (__instance is not ISimpleCalculatedDamageCard simpleCard)
        {
            return __result;
        }

        return
        [
            .. __result,
            .. CustomCardModel.MakeCalculatedDamage(simpleCard.CanonicalCalculatedDamageBase, BonusFunc,
                simpleCard.CanonicalCalculatedDamageMultiplier, simpleCard.DamageType)
        ];
    }

    private static decimal BonusFunc(CardModel card, Creature? target)
    {
        if (card is not ISimpleCalculatedDamageCard simpleCard)
        {
            throw new InvalidOperationException("What? How? Why? This isn't possible");
        }

        return simpleCard.CalculateDamage(target);
    }
}