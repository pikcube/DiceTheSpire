using DiceTheSpireCore.DiceTheSpireCoreCode;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class BlastChiller() : TheInventorCard(-1, CardType.Attack, CardRarity.Rare, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        if (RunState is null || CombatState is null)
        {
            return;
        }

        int damage = Owner.Creature.Block;

        if (IsUpgraded)
        {
            await DamageCmd.Attack(damage)
                .FromCard(this)
                .TargetingAllOpponents(CombatState)
                .WithHitFx(VfxCmd.slashPath)
                .Execute(choiceContext);
        }
        else
        {
            await DamageCmd.Attack(damage)
                .FromCard(this)
                .WithHitCount(1)
                .TargetingRandomOpponents(CombatState)
                .WithHitFx(VfxCmd.slashPath)
                .Execute(choiceContext);
        }
        await DiceyHooks.OnTurnEndInHand(this, RunState, CombatState);
    }

    public override string GetScrapId => nameof(WallOfIce);
}