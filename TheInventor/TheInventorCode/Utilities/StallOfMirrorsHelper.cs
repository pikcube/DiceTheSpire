using BaseLib.Abstracts;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Cards.Rare;

namespace TheInventor.TheInventorCode.Utilities;

public class StallOfMirrorsHelper() : CustomSingletonModel(HookType.Combat)
{
    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        foreach (Player p in participants.Select(c => c.Player).Where(c => c is not null).OfType<Player>())
        {
            int current = StallOfMirrors.CurrentStall.Get(p);
            StallOfMirrors.CurrentStall.Set(p, 0);
            await HallOfMirrorsPower.ApplyAsync(choiceContext, p.Creature, current, p.Creature,
                ModelDb.Card<StallOfMirrors>());
        }
    }
}