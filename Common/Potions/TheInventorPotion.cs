using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using DiceTheSpire.Common.Extensions;
using DiceTheSpire.Inventor;

namespace DiceTheSpire.Common.Potions;

[Pool(typeof(TheInventorPotionPool))]
public abstract class TheInventorPotion : CustomPotionModel
{
    public override string CustomPackedImagePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
    public override string CustomPackedOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".PotionImagePath();
}