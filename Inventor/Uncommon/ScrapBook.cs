using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Listeners;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Inventor.Uncommon;


public class ScrapBook() : TheInventorCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self), IOnShockListener, IScrapCard
{
    public override string GetScrapId => nameof(MagicDice);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(3)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ShockModel.Shock];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.ForEnergy(this)];

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public async Task AfterCardShockedAsync(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card == this)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        }
    }

    public bool IsAlwaysOfferedAsScrap => true;
}