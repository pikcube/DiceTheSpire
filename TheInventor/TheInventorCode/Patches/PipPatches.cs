using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Random;
using System.Data;
using System.Reflection;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Cards;

namespace TheInventor.TheInventorCode.Patches;

[HarmonyPatch(typeof(NCard), "UpdateEnergyCostVisuals")]
public static class PipPatches
{
    public static void Postfix(NCard __instance)
    {
        if (__instance.Model is not TheInventorCard c)
        {
            return;
        }

        FieldInfo energyLabelInfo = AccessTools.DeclaredField(typeof(NCard), "_energyLabel");
        MegaLabel l = (MegaLabel?) energyLabelInfo.GetValue(__instance) ?? throw new NoNullAllowedException();

        FieldInfo energyTexture = AccessTools.DeclaredField(typeof(NCard), "_energyIcon");
        TextureRect r = (TextureRect?)energyTexture.GetValue(__instance) ?? throw new NoNullAllowedException();

        FieldInfo pretendCard = AccessTools.DeclaredField(typeof(NCard), "_pretendCardCanBePlayed");
        bool isPretend = (bool?)pretendCard.GetValue(__instance) ?? throw new NoNullAllowedException();

        int withModifiers = c.EnergyCost.GetWithModifiers(CostModifiers.All);

        if (c.EnergyCost.CostsX)
        {
            l.SetTextAutoSize("X");
            withModifiers = 0;
        }
        else if (withModifiers > 9)
        {
            l.SetTextAutoSize($"{withModifiers}");
        }
        else
        {
            l.SetTextAutoSize("");
        }
        l.Set("theme_override_colors/font_color", new Color(0, 0, 0));
        l.Set("theme_override_colors/font_outline_color", new Color(0, 0, 0, 0));
        l.Set("theme_override_colors/font_shadow_color", new Color(0, 0, 0, 0));


        r.Texture = c.GetPips(withModifiers, isPretend);
    }
}

[HarmonyPatch(typeof(NCard), nameof(NCard.PlayRandomizeCostAnim))]
public static class RandomizePatch
{
    public static bool Prefix(NCard __instance)
    {
        if (__instance.Model is not TheInventorCard c)
        {
            return true;
        }

        __instance.GetPrivateProperty<NCard, Tween>("RandomizeCostTween")?.Kill();

        PropertyInfo randomizeCostTween = AccessTools.DeclaredProperty(typeof(NCard), "RandomizeCostTween");
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