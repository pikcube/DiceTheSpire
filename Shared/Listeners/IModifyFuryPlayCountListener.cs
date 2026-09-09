using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Listeners;

public interface IModifyFuryPlayCountListener
{
    public void ModifyFuryPlayCount(FuryPower furyPower, CardModel card, ref int furyCount);
}