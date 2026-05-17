using BaseLib.Utils;
using DiceTheSpireCore.DiceTheSpireCoreCode.Singletons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;


namespace DiceTheSpireCore.DiceTheSpireCoreCode.Cards;

[Pool(typeof(StatusCardPool))]
public class Splinter() : DiceTheSpireCoreCard(-1, CardType.Status, CardRarity.Status, TargetType.None), IOnDiscardListener
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Unplayable];

    public async Task OnDiscardAsync(PlayerChoiceContext choiceContext)
    {
        await CardCmd.Exhaust(choiceContext, this, false, false);
    }

}