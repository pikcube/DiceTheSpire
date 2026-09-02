//using MegaCrit.Sts2.Core.Commands;
//using MegaCrit.Sts2.Core.Entities.Cards;
//using MegaCrit.Sts2.Core.Entities.Powers;
//using MegaCrit.Sts2.Core.GameActions.Multiplayer;
//using MegaCrit.Sts2.Core.Models.Powers;

//namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
//public class SuperSetDrawPower : DiceTheSpireCorePower
//{
//    public override PowerType Type => PowerType.Buff;
//    public override PowerStackType StackType => PowerStackType.Counter;
//    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        if (cardPlay.Card.Type != CardType.Power || Owner.Player is null)
//        {
//            return;
//        }
//        "DICETHESPIRECORE-SUPER_SET_DRAW_POWER.description": "When you play a [gold]Power[/gold], {Amount}",
//        "DICETHESPIRECORE-SUPER_SET_DRAW_POWER.smartDescription": "",
//        "DICETHESPIRECORE-SUPER_SET_DRAW_POWER.title": "Super Set (Draw)",
//        await CardPileCmd.Draw(choiceContext, Amount, Owner.Player);
//    }
//}