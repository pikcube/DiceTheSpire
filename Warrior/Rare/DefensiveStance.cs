using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Rare;
public class DefensiveStance() : TheWarriorCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(3, BlockProps.card)]; //, new CardsVar(1)
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card == this)
        {
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CreateClone(), PileType.Hand, card.Owner));
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CreateClone(), PileType.Hand, card.Owner));
        }
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

        //await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        base.OnUpgrade();
        DynamicVars.Block.UpgradeValueBy(1);

    }

}
