using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace TheInventor.TheInventorCode.Relics;


public class ToastedMarshmallow : TheInventorRelic
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    private int ModifyNextDraw { get; set; }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player || !(Owner.PlayerCombatState?.Energy > 0))
        {
            return Task.CompletedTask;
        }

        ModifyNextDraw = 1;
        Status = RelicStatus.Active;

        return Task.CompletedTask;
    }

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (Owner != player)
        {
            return count;
        }

        return count + ModifyNextDraw;
    }

    public override Task AfterModifyingHandDraw()
    {
        Status = RelicStatus.Normal;
        if (ModifyNextDraw <= 0)
        {
            return Task.CompletedTask;
        }

        Flash();
        ModifyNextDraw = 0;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        ModifyNextDraw = 0;
        Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }
}