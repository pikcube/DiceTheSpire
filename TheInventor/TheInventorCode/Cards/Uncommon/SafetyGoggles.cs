using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Uncommon;


public class SafetyGoggles() : TheInventorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(Protection);
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SafetyGogglesPower>(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await SafetyGogglesPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Power<SafetyGogglesPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<SafetyGogglesPower>().UpgradeValueBy(1);
    }
}