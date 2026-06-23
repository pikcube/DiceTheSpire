using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Utilities;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class Plague() : TheInventorCard(-1, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
{
    public override string GetScrapId => nameof(Stardust);

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        if (CombatState is null || RunState is null)
        {
            return;
        }

        if (!IsUpgraded)
        {
            await InventorHelperFunctions.ApplyRandomDebuffAsync(choiceContext, RunState, Owner.Creature, Owner.Creature, this);
        }

        foreach (Creature c in CombatState.HittableEnemies)
        {
            await InventorHelperFunctions.ApplyRandomDebuffAsync(choiceContext, RunState, c, Owner.Creature, this);
        }
    }
}