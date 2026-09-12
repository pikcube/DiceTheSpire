using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Shared.DynamicVars;

public class ShockVar(decimal value, string name = "Shock") : DynamicVar(name, value);