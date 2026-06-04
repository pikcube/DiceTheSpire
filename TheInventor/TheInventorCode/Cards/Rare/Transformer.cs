using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Keywords;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class Transformer() : TheInventorCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override string GetScrapId => nameof(Overload);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(CurrentDamage, DamageProps.card), new IntVar("Increase", 3)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [ScrapKeyword.Scrap];

    [SavedProperty]
    public int CurrentDamage
    {
        get;
        set
        {
            AssertMutable();
            field = value;
            DynamicVars.Damage.BaseValue = field;
        }
    } = 2;

    [SavedProperty]
    public int IncreasedDamage
    {
        get;
        set
        {
            AssertMutable();
            field = value;
            UpdateDamage();
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }

    public override Task OnSkippedAsync()
    {
        BuffDamage();
        return Task.CompletedTask;
    }

    private void BuffDamage()
    {
        IncreasedDamage += DynamicVars["Increase"].IntValue;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Increase"].UpgradeValueBy(1);
    }

    protected override void AfterDowngraded()
    {
        UpdateDamage();
    }

    private void UpdateDamage() => CurrentDamage = 2 + IncreasedDamage;
}