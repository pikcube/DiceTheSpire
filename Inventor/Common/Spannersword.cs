using DiceTheSpire.Common.Utility;
using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;

namespace DiceTheSpire.Inventor.Common;

public class Spannersword() : TheInventorCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    public override string GetScrapId => nameof(Hook);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, DamageProps.card), new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(BlinkModel.Blink)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToBlink, DynamicVars.Cards.IntValue, DynamicVars.Cards.IntValue);
        IEnumerable<CardModel> results = await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this);
        await Task.WhenAll(results.Select(card => card.BlinkAsync(choiceContext)));
        IEnumerable<CardModel> toRetrive = PileType.Discard.GetPile(Owner).Cards
            .TakeRandom(DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardSelection);
        await CardPileCmd.Add(toRetrive, PileType.Hand, CardPilePosition.Bottom, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}