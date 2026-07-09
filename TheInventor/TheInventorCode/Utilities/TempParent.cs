using MegaCrit.Sts2.Core.Entities.Players;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Interfaces;

namespace TheInventor.TheInventorCode.Utilities;

internal class TempParent : IGadgetParent
{
    public TempParent(Player player, GadgetModel gadget)
    {
        Owner = player;
        LinkedGadgetModel = gadget.GetMutable(this);
        gadget.OnFirstCharge();
    }

    public string GadgetId
    {
        set {}
    }
    public Player Owner { get; }
    public GadgetModel LinkedGadgetModel { get; internal set; }

    public void Flash()
    {
    }

    public Task AfterRandomizedAsync()
    {
        return Task.CompletedTask;
    }

    public void Update()
    {
    }

    public void SetValue(int display)
    {
    }
}