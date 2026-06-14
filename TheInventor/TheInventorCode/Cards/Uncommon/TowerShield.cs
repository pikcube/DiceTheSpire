using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Uncommon;

public class TowerShield() : TheInventorCard(-1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override bool HasEnergyCostX => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6, BlockProps.card)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int xValue = cardPlay.Card.ResolveEnergyXValue();

        for (int n = 0; n < xValue; ++n)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }
    }

    public override string GetScrapId => nameof(Protection);

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
    }
}
