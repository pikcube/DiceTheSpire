using BaseLib.Abstracts;
using BaseLib.Utils;
using DiceTheSpire.DiceTheSpireCode.Inventor.Rare;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.DiceTheSpireCode.Common.Utility;

[UsedImplicitly]
public class StallOfMirrorsHelper() : CustomSingletonModel(HookType.Combat)
{
    public static readonly SavedSpireField<Player, int> CurrentStall = new(() => 0, $"{MainFile.ModId}_{nameof(CurrentStall)}");

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        foreach (Player p in participants.Select(c => c.Player).Where(c => c is not null).OfType<Player>())
        {
            int current = CurrentStall.Get(p);
            CurrentStall.Set(p, 0);
            await HallOfMirrorsPower.ApplyAsync(choiceContext, p.Creature, current, p.Creature, ModelDb.Card<StallOfMirrors>());
        }
    }
}