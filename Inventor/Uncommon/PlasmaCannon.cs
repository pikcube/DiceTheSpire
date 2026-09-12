using BaseLib.Extensions;
using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Uncommon;

public class PlasmaCannon() : TheInventorCard(3, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(BattleWrench);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PlasmaCannonPower>(1)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Unplayable), HoverTipFactory.FromKeyword(ShockModel.Shock)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlasmaCannonPower.ApplyAsync(choiceContext, Owner, DynamicVars.Power<PlasmaCannonPower>().IntValue, Owner, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<PlasmaCannonPower>().UpgradeValueBy(1);
    }
}