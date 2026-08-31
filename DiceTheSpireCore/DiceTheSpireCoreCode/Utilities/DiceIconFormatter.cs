using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;

public class DiceIconFormatter : IFormatter
{
    public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
    {
        ArgumentNullException.ThrowIfNull(formattingInfo.CurrentValue);
        int result = GetValue(formattingInfo.CurrentValue);

        switch (result)
        {
            case < 1:
                return false;
            case > 9:
            {
                string element = Path.Join("text", "Dicecon_1.png").ImagePath();
                formattingInfo.Write($"{result}[img]{element}[/img]");
                break;
            }
            default:
            {
                string element = Path.Join("text", $"Dicecon_{result}.png").ImagePath();
                formattingInfo.Write($"[img]{element}[/img]");
                break;
            }
        }

        return true;
    }

    private static int GetValue(object value)
    {
        return value switch
        {
            EnergyVar energyVar => Convert.ToInt32(energyVar.PreviewValue),
            CalculatedVar calculatedVar => Convert.ToInt32(calculatedVar.Calculate(null)),
            DynamicVar dynamicVar => Convert.ToInt32(dynamicVar.PreviewValue),
            decimal num1 => (int)num1,
            int num2 => num2,
            string => int.Parse(value.ToString() ?? ""),
            _ => throw new LocException($"Unknown value='{value}' type={value.GetType()}")
        };
    }

    public string Name { get; set; } = "diceIcons";
    public bool CanAutoDetect { get; set; }
}