using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Players;

namespace TheInventor.TheInventorCode.Gadgets;

[UsedImplicitly]
public class BattleWrench() : AbstractGadget(nameof(BattleWrench))
{
    public override decimal ModifyHandDrawLate(Player player, decimal count)
    {
        if (player == Parent?.Owner)
        {
            return count + 2;
        }

        return count;
    }
}