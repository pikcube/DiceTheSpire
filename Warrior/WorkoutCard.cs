using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace DiceTheSpire.Warrior;

[Pool(typeof(EventCardPool))]
public abstract class WorkoutCard(int cost, CardType type, TargetType target) : TheWarriorCard(cost, type, CardRarity.Event, target)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<TheWarriorCardPool>();
}