using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Utility;
using DiceTheSpire.Warrior.Rare;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.WorkoutRewards;
public class KnockoutPunch() : TheWarriorCard(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [.. MakeCalculatedDamage(12, Bonus, 12)];

    private static decimal Bonus(CardModel card, Creature? target)
    {
        return card.RunState?.CurrentRoom?.RoomType is RoomType.Boss or RoomType.Elite ? 3 : 1;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

            await DamageCmd.Attack(DynamicVars.CalculatedDamage.IntValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.bluntPath)
            .Execute(choiceContext);

    }
    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(2);
    }

}
