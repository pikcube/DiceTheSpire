//using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
//using MegaCrit.Sts2.Core.Commands;
//using MegaCrit.Sts2.Core.Entities.Cards;
//using MegaCrit.Sts2.Core.GameActions.Multiplayer;
//using MegaCrit.Sts2.Core.HoverTips;

//namespace TheWarrior.TheWarriorCode.Cards.Basic;


//public class CombatRoll() : TheWarriorCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
//{
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<RollAgain>()];
//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        if (CombatState is null)
//        {
//            return;
//        }

//        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
//        await Cmd.Wait(0.25f);
//        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
//        await Cmd.Wait(0.25f);
//        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
//        await Cmd.Wait(0.25f);


//        //CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner));
//        //await Cmd.Wait(0.25f);
//    }

//    protected override void OnUpgrade()
//    {
//        AddKeyword(CardKeyword.Retain);
//    }

//}
