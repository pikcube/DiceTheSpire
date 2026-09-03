using DiceTheSpire.DiceTheSpireCode.Common.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.DiceTheSpireCode.Common.Listeners;

public interface IModifyFuryPlayCountListener
{
    public void ModifyFuryPlayCount(FuryPower furyPower, CardModel card, ref int furyCount);
}