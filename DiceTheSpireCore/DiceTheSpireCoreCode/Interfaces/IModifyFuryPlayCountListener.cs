using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface IModifyFuryPlayCountListener
{
    public void ModifyFuryPlayCount(FuryPower furyPower, CardModel card, ref int furyCount);
}