using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Interfaces;

public interface IGadgetParent
{
    public string GadgetId { set; }
    public Player Owner { get; }
    public GadgetModel LinkedGadgetModel { get; }
    public AbstractModel AsModel();
    public void Flash();
}