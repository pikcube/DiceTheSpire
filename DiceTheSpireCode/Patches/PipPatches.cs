using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Random;
using Pikcube.Common.Extensions;
using System.Data;
using System.Reflection;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

namespace DiceTheSpire.DiceTheSpireCode.Patches;

[HarmonyPatch(typeof(NCard), "UpdateEnergyCostVisuals")]
public static class PipPatches
{
    public static void Postfix(NCard __instance, MegaLabel ____energyLabel, TextureRect ____energyIcon, bool ____pretendCardCanBePlayed)
    {
        if (__instance.Model is not IPipCard c)
        {
            return;
        }

        int? withModifiers = c.EnergyCost.GetWithModifiers(CostModifiers.All);

        if (c.EnergyCost.CostsX)
        {
            ____energyLabel.SetTextAutoSize("");
            withModifiers = null;
        }
        else if (withModifiers > 9)
        {
            ____energyLabel.SetTextAutoSize($"{withModifiers}");
        }
        else
        {
            ____energyLabel.SetTextAutoSize("");
            ____energyLabel.Set("theme_override_colors/font_color", new Color(0, 0, 0));
            ____energyLabel.Set("theme_override_colors/font_outline_color", new Color(0, 0, 0, 0));
            ____energyLabel.Set("theme_override_colors/font_shadow_color", new Color(0, 0, 0, 0));
        }


        ____energyIcon.Texture = c.GetPips(withModifiers, ____pretendCardCanBePlayed);
    }
}

[HarmonyPatch(typeof(NCard), nameof(NCard.PlayRandomizeCostAnim))]
public static class RandomizePatch
{
    public static bool Prefix(NCard __instance)
    {
        if (__instance.Model is not IPipCard c)
        {
            return true;
        }
        PropertyInfo randomizeCostTween = AccessTools.DeclaredProperty(typeof(NCard), "RandomizeCostTween");

        __instance.GetPrivateProperty<NCard, Tween>("RandomizeCostTween")?.Kill();


        randomizeCostTween.SetValue(__instance, __instance.CreateTween());

        float offset = Rng.Chaotic.NextFloat(10f);
        __instance.GetPrivateProperty<NCard, Tween>("RandomizeCostTween")!.TweenMethod(Callable.From<float>(t =>
        {
            int val = (int)Math.Floor(offset + t) % 8 + 1;

            FieldInfo energyTexture = AccessTools.DeclaredField(typeof(NCard), "_energyIcon");
            TextureRect r = (TextureRect?)energyTexture.GetValue(__instance) ?? throw new NoNullAllowedException();
            r.Texture = c.GetPips(val, false, CardCostColor.Unmodified);

        }), 0, 50, Rng.Chaotic.NextFloat(0.4f, 0.6f)).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        __instance.GetPrivateProperty<NCard, Tween>("RandomizeCostTween")!.Connect(Tween.SignalName.Finished, Callable.From((Action)(() =>
        {
            if (__instance.Model == null)
            {
                return;
            }

            AccessTools.DeclaredMethod(typeof(NCard), "UpdateEnergyCostVisuals", [typeof(PileType)])
                .Invoke(__instance, [__instance.DisplayingPile]);
        })), 4U);

        return false;
    }
}