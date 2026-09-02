using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.DiceTheSpireCode.Common.Relics;


public class ToastedMarshmallow : TheInventorRelic
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override async Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner)
        {
            return;
        }

        int count = player.PlayerCombatState?.Energy ?? 0;

        for (int n = 0; n < count; ++n)
        {
            await CreatureCmd.GainBlock(player.Creature, 2, BlockProps.nonCardUnpowered, null);
        }
        Flash();
    }
}