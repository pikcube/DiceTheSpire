using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Uncommon;


public class Icicle() : TheInventorCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
{
    public override string GetScrapId => nameof(Burrower);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(13, BlockProps.card), new PowerVar<WeakPower>(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null);
        if (CombatState is null)
        {
            return;
        }
        await PowerCmd.Apply<WeakPower>(choiceContext, CombatState.Enemies, DynamicVars.Weak.IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
}