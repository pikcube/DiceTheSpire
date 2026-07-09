using BaseLib.Abstracts;
using BaseLib.Utils;
using TheThief.TheThiefCode.Character;

namespace TheThief.TheThiefCode.Potions;

[Pool(typeof(TheThiefPotionPool))]
public abstract class TheThiefPotion : CustomPotionModel;