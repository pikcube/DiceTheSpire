using DiceTheSpire.DiceTheSpireCode.Inventor.Token;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.DiceTheSpireCode.Powers;

public class AvalanchePower : TheInventorPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (Owner != player.Creature)
        {
            return;
        }

        await CardPileCmd.AddToCombatAndPreview<Rock>(Owner, PileType.Hand, Amount, Owner.Player);
    }
}