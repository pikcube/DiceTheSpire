using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Interfaces;

public interface IWorkoutQuest
{
    public CardModel AsCardModel();
    public CardModel RewardCard { get; }
}