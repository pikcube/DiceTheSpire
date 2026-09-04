using BaseLib.Abstracts;
using BaseLib.Utils;
using DiceTheSpire.Thief;

namespace DiceTheSpire.Shared.Potions;

[Pool(typeof(TheThiefPotionPool))]
public abstract class TheThiefPotion : CustomPotionModel;