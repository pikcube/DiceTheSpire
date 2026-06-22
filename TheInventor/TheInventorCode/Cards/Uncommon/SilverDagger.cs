using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Utilities;

namespace TheInventor.TheInventorCode.Cards.Uncommon;

public class SilverDagger() : TheInventorCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public override string GetScrapId => nameof(Stardust);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, DamageProps.card), new IntVar("DebuffCount", 2)];
    public override IEnumerable<CardTag> Tags => [CardTag.Strike];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        for (int n = 0; n < DynamicVars["DebuffCount"].IntValue; ++n)
        {
            if (RunState is null || cardPlay.Target?.IsAlive is not true)
            {
                return;
            }

            await InventorHelperFunctions.ApplyRandomDebuffAsync(choiceContext, RunState, cardPlay.Target, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars["DebuffCount"].UpgradeValueBy(2);
    }
}