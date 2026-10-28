//using DiceTheSpire.Shared.Commands;
//using DiceTheSpire.Shared.Keywords;
//using DiceTheSpire.Shared.Utility;
//using MegaCrit.Sts2.Core.CardSelection;
//using MegaCrit.Sts2.Core.Commands;
//using MegaCrit.Sts2.Core.Entities.Cards;
//using MegaCrit.Sts2.Core.Extensions;
//using MegaCrit.Sts2.Core.GameActions.Multiplayer;
//using MegaCrit.Sts2.Core.HoverTips;
//using MegaCrit.Sts2.Core.Localization.DynamicVars;
//using MegaCrit.Sts2.Core.Models;
//using MegaCrit.Sts2.Core.ValueProps;

//namespace DiceTheSpire.Warrior.Uncommon;

//public class AcornShield() : TheWarriorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
//{
//    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new BlockVar(5, BlockProps.card)];
//    public override bool GainsBlock => true;
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Nudge)];
//    public override IEnumerable<CardKeyword> CanonicalKeywords => [ReturnModel.Return];
//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

//        if (IsUpgraded)
//        {
//            CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToNudge, 0, DynamicVars.Cards.IntValue);
//            CardModel[] cardChoices = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
//            foreach (CardModel card in cardChoices)
//            {
//                if (!card.EnergyCost.CostsX)
//                {
//                    await NudgeCmd.NudgeAsync(card, NudgeDuration.UntilPlayed);
//                }
//            }
//        }
//        else
//        {
//            CardModel[] cards =
//            [
//                ..PileType.Hand.GetPile(Owner).Cards
//                            .Where(c => !c.Keywords.Contains(CardKeyword.Unplayable) && !c.EnergyCost.CostsX && c.EnergyCost.GetAmountToSpend()!=0)
//                            .TakeRandom((int)DynamicVars.Cards.BaseValue, Owner.RunState.Rng.CombatCardSelection)
//            ];
//            foreach (CardModel card in cards)
//            {
//                await NudgeCmd.NudgeAsync(card, NudgeDuration.UntilPlayed);
//            }
//        }
//    }
//}