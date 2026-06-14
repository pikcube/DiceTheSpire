using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class BeeSting() : TheInventorCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(MagicDice);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new PowerVar<ShockPower>(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await BeeStingPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Energy.IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }
}