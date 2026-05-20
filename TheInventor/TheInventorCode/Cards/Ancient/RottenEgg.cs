using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Keywords;

namespace TheInventor.TheInventorCode.Cards.Ancient;

public class RottenEgg() : TheInventorCard(1, CardType.Power, CardRarity.Ancient, TargetType.Self), ITomeCard
{
    public override int MaxUpgradeLevel => 5;

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        DeckVersion?.AddKeyword(ScrapKeyword.Scrap);
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(1);
    }

    public override string GetScrapId => IsUpgradable ? nameof(RottenGadget) : nameof(CursedGadget);

    public override async Task OnScrapAsync(AbstractGadget linkedGadget)
    {
        if (CurrentUpgradeLevel == 6)
        {
            return;
        }

        RottenEgg newEgg = RottenEgg.CreateInstance(Owner);
        newEgg.UpgradeInternal();
        for (int n = 0; n < CurrentUpgradeLevel; ++n)
        {
            newEgg.UpgradeInternal();
        }
        newEgg.FinalizeUpgradeInternal();
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(newEgg, PileType.Deck));
        linkedGadget.BreakMe();
    }
}