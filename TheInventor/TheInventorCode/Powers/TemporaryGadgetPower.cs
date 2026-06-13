using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
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
        ModHelper.SubscribeForCombatStateHooks("TheInventor.TemporaryGadgetPower", CombatStateHooks);
        ModHelper.SubscribeForRunStateHooks("TheInventor.TemporaryGadgetPower", RunStateHooks);
    }

    private static GadgetModel[] RunStateHooks(RunState state)
    {
        return [.. state.Players.SelectMany(p => p.Creature.Powers.OfType<TemporaryGadgetPower>())
            .Select(g => g.LinkedGadgetModel)
            .Where(g => g.HookType == CustomSingletonModel.HookType.Run)];
    }

    private static GadgetModel[] CombatStateHooks(CombatState state)
    {
        return [.. state.Players.SelectMany(p => p.Creature.Powers.OfType<TemporaryGadgetPower>())
            .Select(g => g.LinkedGadgetModel)
            .Where(g => g.HookType == CustomSingletonModel.HookType.Combat)];
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
        await AfterRandomizedAsync();
    }

    public async Task AfterRandomizedAsync()
    {
        GadgetCard gadgetCard = GadgetCard.Create();
        gadgetCard.SetVars(LinkedGadgetModel);
        await gadgetCard.ShowAndDestoryCardAsync(0.5f);
    }
}
