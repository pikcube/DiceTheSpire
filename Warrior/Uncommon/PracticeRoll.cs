using DiceTheSpire.Common.Cards;
using DiceTheSpire.Common.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Warrior.Uncommon;

public class PracticeRoll() : TheWarriorCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<RollAgain>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DexterityPower>(0M)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<PracticeRollPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}