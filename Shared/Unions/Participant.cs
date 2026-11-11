using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Unions;

public readonly union Participant(Creature, Player, MonsterModel)
{
    public bool IsPlayer => this switch 
    {
        Creature creature => creature.IsPlayer,
        Player => true,
        MonsterModel => false,
        _ => throw new InvalidCastException()
    };

    public static implicit operator Creature(Participant participant) => participant switch
    {
        Creature c => c,
        Player p => p.Creature,
        MonsterModel m => m.Creature,
        _ => throw new InvalidCastException()
    };

    public static implicit operator Participant?(Creature? creature)
    {
        if (creature is null)
        {
            return null;
        }

        return new Participant(creature);
    }

    public static implicit operator Participant?(Player? player)
    {
        if (player is null)
        {
            return null;
        }

        return new Participant(player);
    }

    public static implicit operator Participant?(MonsterModel? monsterModel)
    {
        if (monsterModel is null)
        {
            return null;
        }

        return new Participant(monsterModel);
    }
}