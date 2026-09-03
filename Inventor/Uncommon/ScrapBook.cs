using DiceTheSpire.Common.Interfaces;
using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Keywords;
using Pikcube.Common.Utility;

namespace DiceTheSpire.Inventor.Uncommon;


public class ScrapBook() : TheInventorCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self), IOnBlinkListener, IScrapCard
{
    public override string GetScrapId => nameof(MagicDice);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(3)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BlinkModel.Blink];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.ForEnergy(this)];

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public async Task AfterCardBlinkedAsync(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card == this)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        }
    }

    public bool IsAlwaysOfferedAsScrap => true;
}