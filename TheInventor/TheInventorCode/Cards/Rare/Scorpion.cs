using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class Scorpion() : TheInventorCard(7, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override string GetScrapId => nameof(PoisonArrow);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PoisonPower>(99)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<PoisonPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await PoisonPower.ApplyAsync(choiceContext, cardPlay.Target, DynamicVars.Poison.IntValue, Owner.Creature, this);
    }

    public override bool HasTurnEndInHandEffect => true;
    protected override Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        EnergyCost.AddThisCombat(-1);
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<PoisonPower>().UpgradeValueBy(12);
    }
}