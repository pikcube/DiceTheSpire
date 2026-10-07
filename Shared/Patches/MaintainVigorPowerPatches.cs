using System.Reflection;
using DiceTheSpire.Shared.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(VigorPower), nameof(VigorPower.AfterAttack))]
public static class MaintainVigorPowerPatches
{
    public static bool Prefix(VigorPower __instance, ref Task __result, PlayerChoiceContext choiceContext, AttackCommand command)
    {
        _ = choiceContext;

        if (!__instance.Owner.HasPower<FuryPower>())
        {
            return true;
        }

        PrivateFieldWrapper<PowerModel, object> internalData = __instance.PrivateFieldWrapper<PowerModel, object>("_internalData");


        FieldInfo? declaredField = internalData.Value?.GetType().DeclaredField("commandToModify");
        if (declaredField is null)
        {
            return true;
        }

        if (declaredField.GetValue(internalData.Value) != command)
        {
            return true;
        }

        declaredField.SetValue(internalData.Value, null);

        __result = Task.CompletedTask;
        return false;
    }
}