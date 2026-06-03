using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Cards;

[Pool(typeof(StatusCardPool))]
public class Pip() : DiceTheSpireCoreCard(1, CardType.Status, CardRarity.Status, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Sly, CardKeyword.Exhaust];

    public static async Task<IEnumerable<CardModel>> CreateInHand(
        Player owner,
        int count,
        ICombatState combatState)
    {
        if (count == 0)
            return (IEnumerable<CardModel>)Array.Empty<CardModel>();
        if (CombatManager.Instance.IsOverOrEnding)
            return (IEnumerable<CardModel>)Array.Empty<CardModel>();
        List<CardModel> pips = new List<CardModel>();
        for (int index = 0; index < count; ++index)
            pips.Add((CardModel)combatState.CreateCard<Pip>(owner));
        IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>)pips, PileType.Hand, owner);
        return (IEnumerable<CardModel>)pips;
    }
}