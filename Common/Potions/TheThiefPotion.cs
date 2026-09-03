using BaseLib.Abstracts;
using BaseLib.Utils;
using DiceTheSpire.Thief;

namespace DiceTheSpire.Common.Potions;

[Pool(typeof(TheThiefPotionPool))]
public abstract class TheThiefPotion : CustomPotionModel;