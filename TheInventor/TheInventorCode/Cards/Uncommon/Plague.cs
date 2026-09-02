using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Uncommon;

public class Plague() : TheInventorCard(-1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
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
            await PlaguePower.ApplyAsync(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        }

        foreach (Creature c in CombatState.HittableEnemies)
        {
            await PlaguePower.ApplyAsync(choiceContext, c, 1, Owner.Creature, this);
        }
    }
}