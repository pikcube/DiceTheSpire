using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Uncommon;

public class SawWave() : TheInventorCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(Stardust);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SawWavePower>(1)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<DexterityPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await SawWavePower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Power<SawWavePower>().EnchantedValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<SawWavePower>().UpgradeValueBy(1);
    }
}