using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Uncommon;

public class Solenoid() : TheInventorCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    private const int BaseDamage = 16;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(BaseDamage, DamageProps.card)];

    [SavedProperty]
    public int CombatsWonWithCard { get; set; }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [ShockModel.Shock];

    public override string GetScrapId => nameof(Efficiency);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8);
    }

    public override Task AfterCombatVictory(CombatRoom room)
    {
        CombatsWonWithCard++;
        UpdateValues();
        return Task.CompletedTask;
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        UpdateValues();
        return Task.CompletedTask;
    }

    private void UpdateValues()
    {
        DynamicVars.Damage.BaseValue = Math.Max(BaseDamage - 2 * CombatsWonWithCard + (IsUpgraded ? 8 : 0), 0);
    }

    protected override void AfterCloned()
    {
        base.AfterCloned();
        UpdateValues();
    }
}