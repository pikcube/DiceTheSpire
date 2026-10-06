using MegaCrit.Sts2.Core.Localization;

namespace DiceTheSpire.Shared.Listeners;

public interface IModifyFlavorRelic
{
    public void ModifyFlavor(ref LocString locString);
}