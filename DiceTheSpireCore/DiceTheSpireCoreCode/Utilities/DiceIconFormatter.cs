using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;

public class DiceIconFormatter : IFormatter
{
    public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
    {
        int result;
        switch (formattingInfo.CurrentValue)
        {
            case EnergyVar energyVar:
                result = Convert.ToInt32(energyVar.PreviewValue);
                break;
            case CalculatedVar calculatedVar:
                result = Convert.ToInt32(calculatedVar.Calculate(null));
                break;
            case decimal num1:
                result = (int)num1;
                break;
            case int num2:
                result = num2;
                break;
            case string:
                if (!int.TryParse(formattingInfo.FormatterOptions, out result))
                {
                    return false;
                }
                break;
            default:
                throw new LocException($"Unknown value='{formattingInfo.CurrentValue}' type={formattingInfo.CurrentValue?.GetType()}");
        }

        switch (result)
        {
            case < 1:
                return false;
            case > 9:
            {
                string element = Path.Join("Energy", "ui_dice_dice1.png").ImagePath();
                formattingInfo.Write($"{result}[img]{element}[img]");
                break;
            }
            default:
            {
                string element = Path.Join("Energy", $"ui_dice_dice{result}.png").ImagePath();
                formattingInfo.Write($"[img]{element}[/img]");
                break;
            }
        }

        return true;
    }

    public string Name { get; set; } = "diceIcons";
    public bool CanAutoDetect { get; set; }
}