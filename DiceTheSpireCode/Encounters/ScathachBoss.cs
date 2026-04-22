using DiceTheSpire.DiceTheSpireCode.Monsters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace DiceTheSpire.DiceTheSpireCode.Encounters;

public class ScathachBoss : EncounterModel
{
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
    {
        return [(ModelDb.Monster<Scathach>().ToMutable(), "scathach")];
    }

    public override RoomType RoomType => RoomType.Boss;
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<Scathach>()];
}