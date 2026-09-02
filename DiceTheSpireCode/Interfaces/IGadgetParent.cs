using MegaCrit.Sts2.Core.Entities.Players;

namespace DiceTheSpire.DiceTheSpireCode.Interfaces;

public interface IGadgetParent
{
    public string GadgetId { set; }
    public Player Owner { get; }
    public void Flash();
    public Task AfterRandomizedAsync();
    public void Update();
    void SetValue(int display);
}