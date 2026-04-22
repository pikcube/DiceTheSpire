using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Powers;

namespace DiceTheSpire.DiceTheSpireCode.Monsters;

public class Scathach : MonsterModel
{
    public int AttackDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);

    public int MoveNumber { get; set; } = 0;

    public int RollMoveNumber()
    {
        if (MoveNumber == 0)
        {
            MoveNumber = AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, RunRng.MonsterAi.NextInt(3, 6));
        }
        else
        {
            int newMove = RunRng.MonsterAi.NextInt(1, 6);
            if (MoveNumber == newMove)
            {
                newMove = 6;
            }

            MoveNumber = newMove;
        }

        return MoveNumber;
    }

    public List<MonsterState> States { get; set; } = [];

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        MoveState theCurse = new("thecurse", TheCurse, new SingleAttackIntent(AttackDamage), new DebuffIntent(true));
        MoveState theWind = new("thewind", TheWind, new MultiAttackIntent(AttackDamage, 2), new CardDebuffIntent());
        MoveState theShadow = new("theshadow", TheShadow, new MultiAttackIntent(AttackDamage, 3), new StatusIntent(3));
        MoveState theCold = new("thecold", TheCold, new MultiAttackIntent(AttackDamage, 4), new DebuffIntent());
        MoveState theAshes = new("theashes", TheAshes, new MultiAttackIntent(AttackDamage, 5), new StatusIntent(5));
        MoveState theStorm = new("thestorm", TheStorm, new MultiAttackIntent(AttackDamage,6), new CardDebuffIntent());

        ConditionalBranchState nextMove = new("next");
        nextMove.AddState(theCurse, () => MoveNumber == 1);
        nextMove.AddState(theWind, () => MoveNumber == 2);
        nextMove.AddState(theShadow, () => MoveNumber == 3);
        nextMove.AddState(theCold, () => MoveNumber == 4);
        nextMove.AddState(theAshes, () => MoveNumber == 5);
        nextMove.AddState(theStorm, () => MoveNumber == 6);

        theCurse.FollowUpState = nextMove;
        theWind.FollowUpState = nextMove;
        theShadow.FollowUpState = nextMove;
        theCold.FollowUpState = nextMove;
        theAshes.FollowUpState = nextMove;
        theStorm.FollowUpState = nextMove;

        States = [nextMove, theCurse, theWind, theShadow, theCold, theAshes, theStorm];

        return new MonsterMoveStateMachine(States, States[RollMoveNumber()]);
    }

    //1
    private async Task TheCurse(IReadOnlyList<Creature> creatures)
    {
        await DamageCmd.Attack(AttackDamage).WithHitCount(1).FromMonster(this).Execute(null);
        await PowerCmd.Apply<Cursed>(creatures, 1, Creature, null);
        RollMoveNumber();
    }

    //2
    private async Task TheWind(IReadOnlyList<Creature> creatures)
    {
        await DamageCmd.Attack(AttackDamage).WithHitCount(2).FromMonster(this).Execute(null);

        RollMoveNumber();
    }

    //3
    private async Task TheShadow(IReadOnlyList<Creature> creatures)
    {
        await DamageCmd.Attack(AttackDamage).WithHitCount(3).FromMonster(this).Execute(null);

        RollMoveNumber();
    }

    //4
    private async Task TheCold(IReadOnlyList<Creature> creatures)
    {
        await DamageCmd.Attack(AttackDamage).WithHitCount(4).FromMonster(this).Execute(null);

        RollMoveNumber();
    }

    //5
    private async Task TheAshes(IReadOnlyList<Creature> creatures)
    {
        await DamageCmd.Attack(AttackDamage).WithHitCount(5).FromMonster(this).Execute(null);

        RollMoveNumber();
    }

    //6
    private async Task TheStorm(IReadOnlyList<Creature> creatures)
    {
        await DamageCmd.Attack(AttackDamage).WithHitCount(6).FromMonster(this).Execute(null);

        RollMoveNumber();
    }

    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 636, 600);
    public override int MaxInitialHp => MinInitialHp;
}