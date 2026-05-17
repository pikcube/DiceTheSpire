using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Basic;

public class Capacitor() : TheInventorCard(-1, CardType.Attack, CardRarity.Basic, TargetType.AllEnemies)
{
    public override string OnScrap() => nameof(ShortCircuit);

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        throw new NotImplementedException();
    }
}