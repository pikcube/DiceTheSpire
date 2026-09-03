using System.Data;
using System.Reflection;
using DiceTheSpire.DiceTheSpireCode.Common.Interfaces;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Random;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;

namespace DiceTheSpire.DiceTheSpireCode.Common.Patches;

[HarmonyPatch(typeof(NCard), "UpdateEnergyCostVisuals")]
public static class PipPatches
{
    public static LabelSettings CountdownLabelSettings => new()
    {
        Font = ResourceLoader.Load<FontFile>($"{MainFile.ResPath}/fonts/Pangolin.ttf"),
        FontColor = new Color(0, 0, 0),
        OutlineColor = new Color(0, 0, 0, 0),
        ShadowColor = new Color(0, 0, 0, 0),
    };

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

        if (__instance.Model is ICountdown countdown)
        {
            ____energyLabel.SetTextAutoSize($"{countdown.CurrentCount}");
            ____energyLabel.LabelSettings = CountdownLabelSettings;
            ____energyLabel.LabelSettings.FontSize = Adjust(____energyLabel, ____energyLabel.LabelSettings.Font);
            ____energyIcon.Visible = true;
            ____energyIcon.Texture = c.GetPips(0, ____pretendCardCanBePlayed);
            return;

        }

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

    private static int Adjust(MegaLabel megaLabel, Font themeFont)
    {
        TextParagraph cachedParagraph = (TextParagraph?)AccessTools.DeclaredField(typeof(MegaLabel), "_cachedParagraph").GetValue(null) ?? new TextParagraph();
        float themeConstant = megaLabel.GetThemeConstant(ThemeConstants.Label.LineSpacing, (StringName)"Label");
        Vector2 size = megaLabel.GetRect().Size;
        bool wrap = megaLabel.AutowrapMode != 0;
        int val1 = megaLabel.MinFontSize;
        int val2 = megaLabel.MaxFontSize;
        while (val2 >= val1)
        {
            int fontSize = val1 + (val2 - val1) / 2;
            if (fontSize == megaLabel.MaxFontSize || MegaLabelHelper.IsTooBig(cachedParagraph, megaLabel.Text, themeFont, fontSize, themeConstant, wrap, size))
                val2 = fontSize - 1;
            else
                val1 = fontSize + 1;
        }

        return Math.Min(val1, val2);
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