using DiceTheSpire.Common.Powers;
using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Rare;

public class Screwdriver() : TheInventorCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ScrewdriverPower.ApplyAsync(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string GetScrapId => nameof(LargeToolbox);
}