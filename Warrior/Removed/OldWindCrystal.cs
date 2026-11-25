//using BaseLib.Extensions;
//using DiceTheSpire.Shared.Interfaces;
//using DiceTheSpire.Shared.Powers;
//using DiceTheSpire.Shared.Utility;
//using MegaCrit.Sts2.Core.Commands;
//using MegaCrit.Sts2.Core.Entities.Cards;
//using MegaCrit.Sts2.Core.GameActions.Multiplayer;
//using MegaCrit.Sts2.Core.HoverTips;
//using MegaCrit.Sts2.Core.Localization.DynamicVars;

//namespace DiceTheSpire.Warrior.Uncommon;

//public class WindCrystal() : TheWarriorCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self), ICrystalCard
//{
//    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WindCrystalPower>(1M)];
//    //PowerVar<SuperSetDrawPower>(1M);
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Dodge)];
//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {

//    }
//    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        if (cardPlay.Card == this)
//        {
//            await PowerCmd.Apply<WindCrystalPower>(choiceContext, Owner.Creature, DynamicVars.Power<WindCrystalPower>().IntValue, Owner.Creature, this);
//        }
//    }
//    protected override void OnUpgrade()
//    {
//        DynamicVars.Power<WindCrystalPower>().UpgradeValueBy(1);
//    }
//}


//using BaseLib.Extensions;
//using DiceTheSpire.Shared.Interfaces;
//using DiceTheSpire.Shared.Powers;
//using DiceTheSpire.Shared.Utility;
//using MegaCrit.Sts2.Core.Commands;
//using MegaCrit.Sts2.Core.Entities.Cards;
//using MegaCrit.Sts2.Core.GameActions.Multiplayer;
//using MegaCrit.Sts2.Core.HoverTips;
//using MegaCrit.Sts2.Core.Localization.DynamicVars;

//namespace DiceTheSpire.Warrior.Uncommon;

//public class WindCrystal() : TheWarriorCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self), ICrystalCard
//{
//    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WindCrystalPower>(1M)];
//    //PowerVar<SuperSetDrawPower>(1M);
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Dodge)];
//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {

//    }
//    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        if (cardPlay.Card == this)
//        {
//            await PowerCmd.Apply<WindCrystalPower>(choiceContext, Owner.Creature, DynamicVars.Power<WindCrystalPower>().IntValue, Owner.Creature, this);
//        }
//    }
//    protected override void OnUpgrade()
//    {
//        DynamicVars.Power<WindCrystalPower>().UpgradeValueBy(1);
//    }
//}





//using DiceTheSpire.Shared.Commands;
//using MegaCrit.Sts2.Core.Entities.Cards;
//using MegaCrit.Sts2.Core.Entities.Powers;
//using MegaCrit.Sts2.Core.GameActions.Multiplayer;

//namespace DiceTheSpire.Shared.Powers;

//public class WindCrystalPower : DiceTheSpirePower
//{
//    public override PowerType Type => PowerType.Buff;
//    public override PowerStackType StackType => PowerStackType.Counter;

//    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        if (cardPlay.Card.Type != CardType.Power || Owner.Player is null)
//        {
//            return;
//        }
//        await DodgeCmd.DodgeAsync(choiceContext, Owner.Player, Amount, this);
//    }
//}