using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Utility;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Uncommon;


[UsedImplicitly]
public class Resistor() : TheInventorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(BattleWrench);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(9, BlockProps.card)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [ShockModel.Shock];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(ShockModel.Shock)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        CardSelectorPrefs prefs = new(DiceySelection.ToUnshock, 1, 1);
        CardModel[] cardsToUnshock =
        [
            .. await CardSelectCmd.FromSimpleGrid(choiceContext, ShockModel.ShockedCards(Owner, c => c is not Resistor), Owner, prefs)
        ];
        foreach (CardModel card in cardsToUnshock)
        {
            await card.UnshockAsync();
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
}