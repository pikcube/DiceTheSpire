using MegaCrit.Sts2.Core.Entities.Players;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Interfaces;

public interface IGadgetParent
{
    public string GadgetId { set; }
    public Player Owner { get; }
    public GadgetModel LinkedGadgetModel { get; }
    public void Flash();
    public Task AfterRandomizedAsync();
    public void Update();
}