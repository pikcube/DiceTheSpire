using DiceTheSpire.Shared.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace DiceTheSpire.Shared.Listeners;

public interface IModifyPipOnPlayListener
{
    public bool ShouldModify(Pip pip);
    public LocString PipDescription { get; }
    public IEnumerable<IHoverTip> PipHoverTips { get; }
    public Task ModifyPipOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay, Pip pip);
}