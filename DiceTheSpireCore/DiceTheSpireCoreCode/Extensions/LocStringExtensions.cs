using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;

public static class LocStringExtensions
{
    extension(LocString instance)
    {
        public LocString WithDynamicVars(DynamicVarSet dynamicVars)
        {
            foreach (DynamicVar d in dynamicVars.Values)
            {
                instance.Add(d);
            }

            return instance;
        }
    }
}