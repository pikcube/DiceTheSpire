using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Rare;

public class Transformer() : TheInventorCard(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    private const int Base = 5;

    public override string GetScrapId => nameof(Overload);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(CurrentDamage, DamageProps.card), new BlockVar(CurrentBlock, BlockProps.card), new IntVar("Increase", 2)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

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
    } = Base;

    public int CurrentBlock
    {
        get;
        set
        {
            AssertMutable();
            field = value;
            DynamicVars.Block.BaseValue = field;
        }
    } = Base;

    [SavedProperty]
    public int IncreasedDamageAndBlock
    {
        get;
        set
        {
            AssertMutable();
            field = value;
            UpdateDamage();
            UpdateBlock();
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        if (DeckVersion is Transformer t)
        {
            t.BuffDamageAndBlock();
        }
    }
    private void BuffDamageAndBlock()
    {
        IncreasedDamageAndBlock += DynamicVars["Increase"].IntValue;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Increase"].UpgradeValueBy(1);
    }

    protected override void AfterDowngraded()
    {
        UpdateDamage();
        UpdateBlock();
    }

    private void UpdateDamage() => CurrentDamage = Base + IncreasedDamageAndBlock;
    private void UpdateBlock() => CurrentBlock = Base + IncreasedDamageAndBlock;
}