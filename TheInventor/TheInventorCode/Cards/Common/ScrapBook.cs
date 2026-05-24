using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Keywords;
using Pikcube.Common.Utility;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Keywords;

namespace TheInventor.TheInventorCode.Cards.Common;


public class ScrapBook() : TheInventorCard(2, CardType.Skill, CardRarity.Common, TargetType.Self), IOnBlinkListener
{
    public override string GetScrapId => nameof(MagicDice);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(4)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BlinkModel.Blink, ScrapKeyword.Scrap];

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public async Task AfterCardBlinkedAsync(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card == this)
        {
            await PlayerCmd.GainEnergy(4, Owner);
        }
    }
}