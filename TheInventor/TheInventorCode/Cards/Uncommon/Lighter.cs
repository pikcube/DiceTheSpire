using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Uncommon;

public class Lighter() : TheInventorCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(Crack);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(2), new PowerVar<LighterPower>(50)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await StrengthPower.ApplyAsync(choiceContext, Owner.Creature, -DynamicVars.Strength.EnchantedValue, Owner.Creature, this);
        await LighterPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Power<LighterPower>().EnchantedValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Strength.UpgradeValueBy(-1);
    }
}