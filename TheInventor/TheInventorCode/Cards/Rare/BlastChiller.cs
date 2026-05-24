using DiceTheSpireCore.DiceTheSpireCoreCode;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class BlastChiller() : TheInventorCard(-1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
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

        await CreatureCmd.Damage(choiceContext, CombatState.Enemies, damage, DamageProps.card, Owner.Creature, this);
        await DiceyHooks.OnTurnEndInHand(this, RunState, CombatState);
    }

    public override string GetScrapId => nameof(WallOfIce);


    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}