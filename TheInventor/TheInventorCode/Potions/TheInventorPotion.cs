using BaseLib.Abstracts;
using BaseLib.Utils;
using TheInventor.TheInventorCode.Character;

namespace TheInventor.TheInventorCode.Potions
{
    [Pool(typeof(TheInventorPotionPool))]
    public abstract class TheInventorPotion : CustomPotionModel;
}