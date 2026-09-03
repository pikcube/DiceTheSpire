using DiceTheSpire.Common.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Thief.Uncommon;

public class Upkeep() : TheThiefCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromCard<Pip>(),
        HoverTipFactory.ForEnergy(this)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null || Owner.PlayerCombatState is null)
        {
            return;
        }

        IEnumerable<CardModel> pips = Owner.PlayerCombatState.Hand.Cards.Where((c, _) => c is Pip);
        foreach (CardModel card in pips.ToArray())
        {
            await CardCmd.Exhaust(choiceContext, card);
            await PlayerCmd.GainEnergy(1, Owner);
            await Cmd.Wait(0.5f);
        }

    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}