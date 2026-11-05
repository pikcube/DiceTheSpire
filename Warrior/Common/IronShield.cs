using DiceTheSpire.Shared.DynamicVars;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Common;

public class IronShield() : TheWarriorCard(2, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12, BlockProps.card), .. RangeVars.Make(0, 0)];

    //protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Dodge)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

        //CardSelectorPrefs cardSelectorPrefs = new(CardSelectorPrefs.DiscardSelectionPrompt, 0, DynamicVars.Cards.IntValue);
        //CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
        //foreach (CardModel card in cards)
        //{
        //    await CardCmd.Discard(choiceContext, card);
        //}

        //if (cards.Length == 0)
        //{
        //    return;
        //}


        //await CardPileCmd.Draw(choiceContext, cards.Length, Owner);
    }

    protected override void OnUpgrade()
    {
        //DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars.Block.UpgradeValueBy(3);
    }

}