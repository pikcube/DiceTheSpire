using BaseLib.Abstracts;
using DiceTheSpire.Thief.Common;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Shared.Powers;

public class CloakPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Cloak>();

    protected override bool IsPositive => false;
}