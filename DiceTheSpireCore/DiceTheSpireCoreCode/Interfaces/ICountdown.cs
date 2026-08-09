using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;

public interface ICountdown
{
    public int MaxCount { get; set; }
    public int CurrentCount { get; set; }
    public Player Owner { get;}
    public CardLocation PublicGetResultLocationForCardPlay();
    public Task<int> PublicGeneratePlayCount(ICombatState combatState, Creature? target);
    public Task PublicOnPlay(BranchingPlayerChoiceContext branchingPlayerChoiceContext, CardPlay cardPlay);
}