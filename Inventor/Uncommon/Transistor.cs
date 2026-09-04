using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Listeners;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Uncommon;

public class Transistor() : TheInventorCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy), IAfterCardShockedListener
{
    public override string GetScrapId => nameof(ShortCircuit);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(11, DamageProps.card)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<ShockPower>(1)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.lightningPath)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }

    public async Task AfterCardShockedAsync(PlayerChoiceContext choiceContext, ShockPower shock, CardModel card)
    {
        if (card != this)
        {
            return;
        }

        shock.Cards.Remove(this);
        card.RemoveTempKeywordEarly(CardKeyword.Unplayable);
        card.RemoveKeyword(CardKeyword.Unplayable);
        await CardCmd.AutoPlay(choiceContext, this, null);
    }
}