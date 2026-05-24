using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;

public static class DynamicVarSetExtension
{
    extension(DynamicVarSet instance)
    {
        public DynamicVarSet WithOwnerInitialized(AbstractModel owner)
        {
            instance.InitializeWithOwner(owner);
            return instance;
        }
    }
}