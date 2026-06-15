using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheInventor.TheInventorCode.Powers;

public class FlippedDrawPilePower : TheInventorPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            if (Owner.Player is null)
            {
                return [];
            }

            IReadOnlyList<CardModel> readOnlyList = PileType.Draw.GetPile(Owner.Player).Cards;
            if (readOnlyList.Count == 0)
            {
                return [];
            }

            return [HoverTipFactory.FromCard(readOnlyList[0])];
        }
    }
}