using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class Backfire() : TheInventorCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(InfinityMirror);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new MaxHpVar(3)];

    public override bool CanBeGeneratedInCombat => false;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.EnchantedValue);
        await BackfirePower.ApplyAsync(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }


    protected override void OnUpgrade()
    {
        DynamicVars.MaxHp.UpgradeValueBy(2);
    }
}