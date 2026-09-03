using BaseLib.Abstracts;
using BaseLib.Utils;
using DiceTheSpire.DiceTheSpireCode.Warrior;

namespace DiceTheSpire.DiceTheSpireCode.Common.Potions;

[Pool(typeof(TheWarriorPotionPool))]
public abstract class TheWarriorPotion : CustomPotionModel;