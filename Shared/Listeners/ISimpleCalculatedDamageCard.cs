using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System.Reflection;
using JetBrains.Annotations;

namespace DiceTheSpire.Shared.Listeners;

public interface ISimpleCalculatedDamageCard
{
    /// <summary>
    /// The flat damage always present on the attack.<br/>
    /// Can be modified with DynamicVars.CalculationBase.UpgradeValueBy()
    /// </summary>
    public int BaseDamage { get; }

    /// <summary>
    /// The scaler used to scale the result of <see cref="CalculateDamage"/><br/>
    /// Can be modified with DynamicVars.ExtraDamage.UpgradeValueBy()
    /// </summary>
    public int ExtraDamageMultiplier { get; }

    /// <summary>
    /// The damage props of the move.<br/>
    /// This is usually DamageProps.card.
    /// </summary>
    public ValueProp DamageType { get; }

    /// <summary>
    /// The funciton that calculates the damage.
    /// </summary>
    /// <param name="target">The single target if the attack only targets a single enemy. Null for multi target attacks.</param>
    /// <returns>The damage to be multiplied by <see cref="ExtraDamageMultiplier"/> and added to <see cref="BaseDamage"/></returns>
    public decimal CalculateDamage(Creature? target);
}

[HarmonyPatch]
public static class SimpleCalculatedDamagePatch
{
    [UsedImplicitly]
    private static IEnumerable<MethodBase> TargetMethods()
    {
        MethodInfo? afterAppliedMethod = typeof(CardModel).DeclaredProperty("CanonicalVars")?.GetMethod;
        if (afterAppliedMethod is not null)
        {
            yield return afterAppliedMethod;
        }

        foreach (Type? type in AccessTools.AllTypes()
                     .Where(t => t.IsSubclassOf(typeof(CardModel)) && !t.IsAbstract))
        {
            MethodInfo? method = type.GetProperty("CanonicalVars",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetMethod;

            if (method != null && method.DeclaringType != typeof(CardModel))
            {
                yield return method;
            }
        }
    }

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
            .. CustomCardModel.MakeCalculatedDamage(simpleCard.BaseDamage, BonusFunc,
                simpleCard.ExtraDamageMultiplier, simpleCard.DamageType)
        ];
    }

    private static decimal BonusFunc(CardModel card, Creature? target)
    {
        return card is ISimpleCalculatedDamageCard simpleCard
            ? simpleCard.CalculateDamage(target)
            : throw new InvalidOperationException("What? How? Why? This isn't possible");
    }
}