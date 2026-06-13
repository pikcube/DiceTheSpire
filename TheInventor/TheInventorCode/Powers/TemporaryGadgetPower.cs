using System.Data;
using BaseLib.Abstracts;
using Godot;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Cards.Token;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Interfaces;
using TheInventor.TheInventorCode.Relics;

namespace TheInventor.TheInventorCode.Powers;

[UsedImplicitly]
public class TemporaryGadgetPower : TheInventorPower, IGadgetParent
{
    static TemporaryGadgetPower()
    {
        ModHelper.SubscribeForCombatStateHooks("TheInventor.TemporaryGadgetPower", state => state.Players
            .SelectMany(p => p.Creature.Powers.OfType<TemporaryGadgetPower>())
            .Select(g => g.LinkedGadgetModel)
            .Where(g => g.HookType == CustomSingletonModel.HookType.Combat)
            .ToArray()
        );
        ModHelper.SubscribeForRunStateHooks("TheInventor.TemporaryGadgetPower", state => state.Players
            .SelectMany(p => p.Creature.Powers.OfType<TemporaryGadgetPower>())
            .Select(g => g.LinkedGadgetModel)
            .Where(g => g.HookType == CustomSingletonModel.HookType.Run)
            .ToArray()
        );
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(nameof(GadgetText))];

    private string GadgetText
    {
        get => ((StringVar)DynamicVars[nameof(GadgetText)]).StringValue;
        set => ((StringVar)DynamicVars[nameof(GadgetText)]).StringValue = value;
    }

    public string GadgetId
    {
        get;
        set
        {
            field = value;
            GadgetText = $"{LinkedGadgetModel.GadgetText}";
        }
    } = nameof(DefaultGadget);

    Player IGadgetParent.Owner => Owner.Player ?? throw new InvalidOperationException();
    void IGadgetParent.Flash() => Flash();

    public AbstractModel AsModel() => this;

    public GadgetModel LinkedGadgetModel
    {
        get
        {
            if (field?.GadgetId != GadgetId || field.Parent != this)
            {
                field = Gadget.AllGadgets[GadgetId].GetMutable(this);
            }
            return field;
        }
    }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player is null)
        {
            await PowerCmd.Remove(this);
            return;
        }

        GadgetId = Gadget.GetRandomCombatGadgetId(Owner.Player.RunState.Rng.CombatOrbGeneration);

        BlockingPlayerChoiceContext context = new();

        GadgetCard gadgetCard = GadgetCard.Create();
        gadgetCard.SetVars(LinkedGadgetModel);

        ShowAndDestoryCard(gadgetCard, 0.5f);


        await LinkedGadgetModel.OnRechargeAsync(context, Owner.Player);
    }

    private static void ShowAndDestoryCard(GadgetCard card, float delayTimeBasedOnIndex)
    {
        Control cardPreviewContainer = NRun.Instance?.GlobalUi.CardPreviewContainer ?? throw new NoNullAllowedException();
        NCard nCard = NCard.Create(card) ?? throw new NoNullAllowedException();
        cardPreviewContainer.AddChildSafely(nCard);
        nCard.UpdateVisuals(PileType.Exhaust, CardPreviewMode.Normal);
        Tween tween = nCard.CreateTween();
        tween.TweenProperty(nCard, (NodePath)"scale", Vector2.One, 0.25)
            .From(Vector2.Zero)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Cubic);
        tween.TweenInterval(delayTimeBasedOnIndex);
        tween.TweenCallback(Callable.From((Action)(() => { NRun.Instance.GlobalUi.AddChildSafely(NExhaustVfx.Create(nCard)!); })));
        tween.TweenProperty(nCard, (NodePath)"modulate", StsColors.exhaustGray,
            SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.20000000298023224 : 0.30000001192092896);
        tween.TweenCallback(Callable.From(nCard.QueueFree));
        tween.TweenCallback(Callable.From(card.ResetVars));
    }
}
