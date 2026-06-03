using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Cards;

//I added braces so I could read your code
[Pool(typeof(StatusCardPool))]
public class Pip() : DiceTheSpireCoreCard(1, CardType.Status, CardRarity.Status, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Sly, CardKeyword.Exhaust];

    //Async functions should be suffixed with Async. I don't care if Spire 2 gets this wrong, I have principles
    public static async Task<IEnumerable<CardModel>> CreateInHandAsync(
        Player owner,
        int count,
        ICombatState combatState)
    {
        if (count == 0 || CombatManager.Instance.IsOverOrEnding)
        {
            //Your function returns an itterable, so we don't need to allocate an entire empty array, just an object that itterates over nothing
            //return Array.Empty<CardModel>(); 

            //Before .net8, you'd do this with `return Enumerable.Empty<CardModel>();`, but now we have collection expressions which do the work for us
            return [];
        }

        //A collection expression is preferred to an explicit constructor because it lets the compiler figure out the best way to create this list.
        //List<CardModel> pips = new List<CardModel>();
        List<CardModel> pips = [];
        //Technically this could be an array since we know the size, but that's not an optimization worth chasing unless there's an actual performance gain

        //There are other ways to populate a collection, but I don't think they're actually more readable
        for (int index = 0; index < count; ++index)
        {
            pips.Add(combatState.CreateCard<Pip>(owner));
        }

        //We don't use the assignment, so let's not store it.
        //IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat(pips, PileType.Hand, owner);
        await CardPileCmd.AddGeneratedCardsToCombat(pips, PileType.Hand, owner);

        return pips;
    }
}