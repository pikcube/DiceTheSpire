using DiceTheSpire.Common.Cards;
using DiceTheSpire.Common.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Warrior.Rare;

public class MagicReroll() : TheWarriorCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<MagicRerollPower>(1M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<RollAgain>(IsUpgraded)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<MagicRerollPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}