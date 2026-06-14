using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Interfaces;
using TheInventor.TheInventorCode.Relics;

namespace TheInventor.TheInventorCode.Powers;


public class ScrewdriverPower : TheInventorPower, IGadgetPowerListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public decimal ModifyGadgetPowerMultiplicative(Player owner)
    {
        return Owner == owner.Creature ? 2 : 1;
    }

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player is null)
        {
            await PowerCmd.Remove(this);
            return;
        }
        List<IGadgetParent> gadgetParents = Gadget.GetGadgetParents(Owner.Player);

        if (gadgetParents.Count == 0)
        {
            TemporaryGadgetPower? p = await TemporaryGadgetPower.ApplyAsync(new BlockingPlayerChoiceContext(), Owner, 1, Owner, cardSource);
            if (p is null)
            {
                return;
            }
            await p.RandomizeThis();
        }
    }
}

public interface IGadgetPowerListener
{
    public decimal ModifyGadgetPowerMultiplicative(Player owner);
}