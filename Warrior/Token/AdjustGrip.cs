using BaseLib.Utils;
using DiceTheSpire.Shared.DynamicVars;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Warrior.Token;

[Pool(typeof(TokenCardPool))]
public class AdjustGrip() : TheWarriorCard(3, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [.. RangeVars.Make(0, 0)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Sly];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<ReadjustGrip>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }
        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<ReadjustGrip>(Owner), PileType.Hand, Owner);

    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}