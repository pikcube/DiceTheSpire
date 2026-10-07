using DiceTheSpire.Shared.Cards;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace DiceTheSpire.Warrior.Uncommon;

public class PracticeRoll() : TheWarriorCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<RollAgain>(), HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<PracticeRollPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}