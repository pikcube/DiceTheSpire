using BaseLib.Abstracts;
using BaseLib.Utils;
using TheWarrior.TheWarriorCode.Character;

namespace TheWarrior.TheWarriorCode.Potions;

[Pool(typeof(TheWarriorPotionPool))]
public abstract class TheWarriorPotion : CustomPotionModel;