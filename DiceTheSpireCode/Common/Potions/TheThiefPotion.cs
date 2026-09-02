using BaseLib.Abstracts;
using BaseLib.Utils;
using DiceTheSpire.DiceTheSpireCode.Thief;

namespace DiceTheSpire.DiceTheSpireCode.Common.Potions;

[Pool(typeof(TheThiefPotionPool))]
public abstract class TheThiefPotion : CustomPotionModel;