using BaseLib.Abstracts;
using BaseLib.Utils;
using DiceTheSpire.Warrior;

namespace DiceTheSpire.Shared.Potions;

[Pool(typeof(TheWarriorPotionPool))]
public abstract class TheWarriorPotion : CustomPotionModel;