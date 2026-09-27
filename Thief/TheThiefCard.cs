using BaseLib.Utils;
using DiceTheSpire.Shared.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace DiceTheSpire.Thief;

[Pool(typeof(TheThiefCardPool))]
public abstract class TheThiefCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    DiceTheSpireCard(cost, type, rarity, target)
{
}