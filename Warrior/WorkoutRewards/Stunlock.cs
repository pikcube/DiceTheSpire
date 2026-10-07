using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;
using BaseLib.Extensions;
using DiceTheSpire.Shared.Extensions;


namespace DiceTheSpire.Warrior.WorkoutRewards;
public class Stunlock() : TheWarriorCard(3, CardType.Skill, CardRarity.Token, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [StunIntent.GetStaticHoverTip()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [.. RangeVars.Make(0, 2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await CreatureCmd.Stun(cardPlay.Target);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.MaxRange.UpgradeValueBy(-2);
    }

}
