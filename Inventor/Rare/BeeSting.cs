using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Rare;

public class BeeSting() : TheInventorCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(MagicDice);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new PowerVar<ShockPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.ForEnergy(this), HoverTipFactory.FromPower<ShockPower>(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await HallOfMirrorsPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Energy.IntValue, Owner.Creature, this);
        await BeeStingPower.ApplyAsync(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }
}