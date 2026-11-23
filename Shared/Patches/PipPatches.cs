using System.Data;
using System.Reflection;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Random;
using Pikcube.Common.Code.Extensions;
using Pikcube.Common.Code.Utility;

namespace DiceTheSpire.Shared.Patches;

[HarmonyPatch(typeof(NCard), "UpdateEnergyCostVisuals")]
public static class PipPatches
{

    public static void Prefix(MegaLabel ____energyLabel)
    {
        ____energyLabel.LabelSettings = null;
    }

    public static void Postfix(NCard __instance, MegaLabel ____energyLabel, TextureRect ____energyIcon, bool ____pretendCardCanBePlayed)
    {
        if (__instance.Model is not IPipCard c)
        {
            return;
        }

        int? withModifiers = c.EnergyCost.GetWithModifiers(CostModifiers.All);


        if (__instance.Model.Keywords.Contains(CardKeyword.Unplayable))
        {
            ____energyLabel.SetTextAutoSize("");
            ____energyLabel.Set("theme_override_colors/font_color", new Color(0, 0, 0));
            ____energyLabel.Set("theme_override_colors/font_outline_color", new Color(0, 0, 0, 0));
            ____energyLabel.Set("theme_override_colors/font_shadow_color", new Color(0, 0, 0, 0));
            ____energyIcon.Visible = false;
            return;
        }

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

[HarmonyPatch(typeof(NCard), "UpdateStarCostVisuals")]
public static class StarReplacementPatches
{
    private static Texture2D? CachedStars { get; set; }
    private static LabelSettings CountdownLabelSettings => new()
    {
        Font = ResourceLoader.Load<FontFile>($"{MainFile.ResPath}/fonts/Pangolin.ttf"),
        FontColor = new Color(0, 0, 0),
        OutlineColor = new Color(0, 0, 0, 0),
        ShadowColor = new Color(0, 0, 0, 0),
    };

    public static void Postfix(NCard __instance, MegaLabel ____starLabel, TextureRect ____starIcon)
    {
        CachedStars ??= ____starIcon.Texture;

        ____starLabel.LabelSettings = null;
        ____starIcon.Texture = CachedStars;

        if (__instance.Model is not ICountdown c)
        {
            return;
        }

        if (__instance.Model?.CanonicalStarCost >= 0)
        {
            return;
        }

        ____starIcon.Visible = true;
        ____starLabel.LabelSettings = CountdownLabelSettings;
        ____starLabel.SetTextAutoSize(c.GetCountString());

        ____starIcon.Texture = ResourceLoader.Load<Texture2D>(PipCard.EnergyDiceconPath(MainFile.ResPath, $"ui_dice_dice0.png"));
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

        PrivatePropertyWrapper<NCard, Tween> privatePropertyWrapper = __instance.PrivatePropertyWrapper<NCard, Tween>("RandomizeCostTween");
        privatePropertyWrapper.Value?.Kill();


        privatePropertyWrapper.Value = __instance.CreateTween();

        float offset = Rng.Chaotic.NextFloat(10f);
        privatePropertyWrapper.Value.TweenMethod(Callable.From<float>(t =>
        {
            int val = (int)Math.Floor(offset + t) % 8 + 1;

            FieldInfo energyTexture = AccessTools.DeclaredField(typeof(NCard), "_energyIcon");
            TextureRect r = (TextureRect?)energyTexture.GetValue(__instance) ?? throw new NoNullAllowedException();
            r.Texture = c.GetPips(val, false, CardCostColor.Unmodified);

        }), 0, 50, Rng.Chaotic.NextFloat(0.4f, 0.6f)).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

        privatePropertyWrapper.Value.Connect(Tween.SignalName.Finished, Callable.From((Action)(() =>
        {
            if (__instance.Model == null)
            {
                return;
            }

            AccessTools.DeclaredMethod(typeof(NCard), "UpdateEnergyCostVisuals")
                .Invoke(__instance, [__instance.DisplayingPile]);
        })), 4U);

        return false;
    }
}