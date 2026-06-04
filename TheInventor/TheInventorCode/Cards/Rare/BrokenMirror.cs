using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Commands;
using Pikcube.Common.Extensions;
using Pikcube.Common.Powers;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Rare;


public class BrokenMirror() : TheInventorCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(InfinityMirror);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new PowerVar<CursedPower>(1)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<CursedPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await BrokenMirrorPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Energy.IntValue, Owner.Creature, this);
        await JinxCmd.JinxAsync(choiceContext, Owner.Creature, 1, false, Description, StartOfTurnAsync, Owner.Creature, this);
        EnergyCost.AddThisCombat(1);
    }

    private async Task StartOfTurnAsync(PlayerChoiceContext choiceContext, Creature target)
    {
        await CursedPower.ApplyAsync(choiceContext, target, DynamicVars.Power<CursedPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}