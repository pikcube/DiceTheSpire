using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Common;
public class Glue() : TheWarriorCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new BlockVar(6, BlockProps.card)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain), HoverTipFactory.Static(BetterStaticHoverTips.Rummage)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

        Glue glue = this;
        CardSelectorPrefs prefs = new(SelectionScreenPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, (Func<CardModel, bool>)(c => !c.Keywords.Contains(CardKeyword.Retain)), glue)).FirstOrDefault();
        if (card == null)
            return;
        CardCmd.ApplyKeyword(card, CardKeyword.Retain);
    }

    protected override void OnUpgrade()
    {
        base.OnUpgrade();
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars.Block.UpgradeValueBy(2);
    }

}