using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Players;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Interfaces;
using TheInventor.TheInventorCode.Relics;

namespace TheInventor.TheInventorCode.Powers;

public class TemporaryGadgetPower : TheInventorPower, IGadgetParent
{
    static TemporaryGadgetPower()
    {
        ModHelper.SubscribeForCombatStateHooks("TheInventor.TemporaryGadgetPower", state => state.Players
            .SelectMany(p => p.Creature.Powers.OfType<TemporaryGadgetPower>())
            .Select(g => g.LinkedGadgetModel).ToArray());
        ModHelper.SubscribeForRunStateHooks("TheInventor.TemporaryGadgetPower", state => state.Players
            .SelectMany(p => p.Creature.Powers.OfType<TemporaryGadgetPower>())
            .Select(g => g.LinkedGadgetModel).ToArray());
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
        await LinkedGadgetModel.OnRechargeAsync(new BlockingPlayerChoiceContext(), Owner.Player);
    }
}