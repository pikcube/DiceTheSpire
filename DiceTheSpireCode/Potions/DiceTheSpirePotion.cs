using BaseLib.Abstracts;
using BaseLib.Utils;
using DiceTheSpire.DiceTheSpireCode.Character;

namespace DiceTheSpire.DiceTheSpireCode.Potions
{
    [Pool(typeof(TheInventorPotionPool))]
    public abstract class DiceTheSpirePotion : CustomPotionModel;
}