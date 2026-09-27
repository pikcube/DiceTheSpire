//using DiceTheSpire.Shared.Commands;
//using DiceTheSpire.Shared.Interfaces;
//using DiceTheSpire.Warrior.Rare;
//using MegaCrit.Sts2.Core.Combat.History.Entries;
//using MegaCrit.Sts2.Core.Commands;
//using MegaCrit.Sts2.Core.Entities.Cards;
//using MegaCrit.Sts2.Core.Entities.Creatures;
//using MegaCrit.Sts2.Core.GameActions.Multiplayer;
//using MegaCrit.Sts2.Core.Localization.DynamicVars;
//using MegaCrit.Sts2.Core.Models;

//namespace DiceTheSpire.Warrior.Common;

//public class IceCrystal() : TheWarriorCard(1, CardType.Skill, CardRarity.Common, TargetType.Self), ICrystalCard
//{
//    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
//    public int EnergySpentThisTurn { get; set; }
//    protected override IEnumerable<DynamicVar> CanonicalVars => [.. MakeCalculatedBlock(3, Bonus, 2)];
//    public override bool GainsBlock => true;
//    private static decimal Bonus(CardModel arg1, Creature? arg2)
//    {
//        return arg1 is not IceCrystal ic ? 1 : ic.EnergySpentThisTurn;
//    }
//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(Owner.Creature), cardPlay);
//    }

//    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        EnergySpentThisTurn = EnergySpentThisTurn + cardPlay.Card.EnergyCost.GetAmountToSpend();
//        return base.AfterCardPlayed(choiceContext, cardPlay);
//    }
//    protected override void OnUpgrade()
//    {
//        RemoveKeyword(CardKeyword.Exhaust);
//    }
//}