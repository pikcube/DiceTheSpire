using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Common;

public class Keyblade() : TheInventorCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    public override string GetScrapId => nameof(Hook);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, DamageProps.card), new CardsVar(3)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(BlinkModel.Blink)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        CardModel[] topCards = [.. PileType.Draw.GetPile(Owner).Cards.Take(DynamicVars.Cards.IntValue)];

        if (topCards.Length == 0)
        {
            return;
        }

        CardSelectorPrefs prefs = new(new LocString("card_selection", "TO_BLINK"), 0, topCards.Length);

        IEnumerable<CardModel> selectedCards = await CardSelectCmd.FromSimpleGrid(choiceContext, topCards, Owner, prefs);

        foreach (CardModel card in selectedCards)
        {
            await card.BlinkAsync(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(2);
    }
}