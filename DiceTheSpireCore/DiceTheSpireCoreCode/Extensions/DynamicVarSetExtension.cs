using DiceTheSpireCore.DiceTheSpireCoreCode.DynamicVars;
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

        public MinRangeVar MinRange => instance.Values.OfType<MinRangeVar>().Single();
        public MaxRangeVar MaxRange => instance.Values.OfType<MaxRangeVar>().Single();
    }
}