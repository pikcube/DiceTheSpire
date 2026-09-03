using BaseLib.Extensions;
using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Ancient;

public class Scorpion() : TheInventorCard(6, CardType.Skill, CardRarity.Ancient, TargetType.AnyEnemy)
{
    public override string GetScrapId => nameof(PoisonArrow);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PoisonPower>(36)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<PoisonPower>()];

    public override bool CanBeGeneratedInCombat => false;
    public override bool CanBeGeneratedByModifiers => false;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await PoisonPower.ApplyAsync(choiceContext, cardPlay.Target, DynamicVars.Poison.IntValue, Owner.Creature, this);
        PlayerCmd.EndTurn(Owner, false);
    }

    public override bool HasTurnEndInHandEffect => true;
    protected override Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        EnergyCost.AddThisCombat(-1);
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<PoisonPower>().UpgradeValueBy(13);
    }
}