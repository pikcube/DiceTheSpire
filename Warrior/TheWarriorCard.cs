using BaseLib.Utils;
using DiceTheSpire.Common.Cards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace DiceTheSpire.Warrior;

[Pool(typeof(TheWarriorCardPool))]
public abstract class TheWarriorCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    DiceTheSpireCard(cost, type, rarity, target)
{
    public CardLocation PublicGetResultLocationForCardPlay() => GetResultLocationForCardPlay();

    public Task<int> PublicGeneratePlayCount(ICombatState combatState, Creature? target) => GeneratePlayCount(combatState, target);

    public Task PublicOnPlay(BranchingPlayerChoiceContext branchingPlayerChoiceContext, CardPlay cardPlay) => OnPlay(branchingPlayerChoiceContext, cardPlay);
}