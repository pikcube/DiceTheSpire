using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Runs;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;

public interface IModifyPipOnPlayListener
{
    public Player Owner { get; }
    public LocString PipDescription { get; }
    public IEnumerable<IHoverTip> PipHoverTips { get; }

    public Task ModifyPipOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay);
}