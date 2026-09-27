using BaseLib.Utils;
using DiceTheSpire.Shared.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace DiceTheSpire.Warrior;

[Pool(typeof(TheWarriorCardPool))]
public abstract class TheWarriorCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    DiceTheSpireCard(cost, type, rarity, target)
{
}