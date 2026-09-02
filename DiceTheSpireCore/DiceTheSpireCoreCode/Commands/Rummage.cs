//using MegaCrit.Sts2.Core.CardSelection;
//using MegaCrit.Sts2.Core.Commands;
//using MegaCrit.Sts2.Core.Models;
//using MegaCrit.Sts2.Core.Nodes.Cards;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DiceTheSpireCore.DiceTheSpireCoreCode.Commands
//{
//    internal class Rummage
//    {

//        CardSelectorPrefs cardSelectorPrefs = new(CardSelectorPrefs.DiscardSelectionPrompt, 0, DynamicVars.Cards.IntValue);
//        CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
//        foreach (CardModel card in cards)
//        {
//            await CardCmd.Discard(choiceContext, card);
//    }

//        if (cards.Length == 0)
//        {
//            return;
//        }
//        await CardPileCmd.Draw(choiceContext, cards.Length, Owner);
//    }
//}
