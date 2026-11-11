using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Code.Extensions;

namespace DiceTheSpire.Shared.Extensions;

public static class PowerModelExtensions
{
    extension<T>(T instance) where T : PowerModel
    {
        public PowerModel CanonicalInstance => ModelDb.GetModel<T>(instance.GetType());
    }
}