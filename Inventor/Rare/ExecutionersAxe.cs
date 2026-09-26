using BaseLib.Extensions;
using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Rare;

public class ExecutionersAxe() : TheInventorCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override string GetScrapId => nameof(DialUpSounds);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9, DamageProps.card)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips =>
        [.. HoverTipFactory.FromPowerWithPowerHoverTips<HauntPower>()];

    private Dictionary<CardPlay, int> CountDictionary { get; set; } = [];

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card == this && cardPlay.IsFirstInSeries)
        {
            CountDictionary[cardPlay] = CombatState?.HittableEnemies.Count ?? 1;
        }

        return Task.CompletedTask;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(CountDictionary[cardPlay])
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithValueProp(DynamicVars.Damage.Props)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }

    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card != this)
        {
            return;
        }

        for (int n = 0; n < CountDictionary[cardPlay]; ++n)
        {
            await HauntPower.ApplyAsync(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        }
    }

    protected override void AfterCloned()
    {
        base.AfterCloned();
        CountDictionary = [];
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}