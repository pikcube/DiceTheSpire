using DiceTheSpire.Shared.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace DiceTheSpire.Shared.Listeners;

public interface IModifyPipOnPlayListener
{
    /// <summary>
    /// Return true if this model should used to modify this Pip.
    /// Mainly used to check that the Pip Owner matches the model Owner.
    /// </summary>
    /// <param name="pip">The pip that we are attempting to modify.</param>
    /// <returns></returns>
    public bool ShouldModify(Pip pip);

    /// <summary>
    /// Additional text to describe how the Pip's OnPlay method changes when we modify it.
    /// </summary>
    public LocString PipDescription { get; }

    /// <summary>
    /// Additional HoverTips for Pips that we modify.
    /// </summary>
    public IEnumerable<IHoverTip> PipHoverTips { get; }

    /// <summary>
    /// Executed when a Pip is played and ShouldModify returns true.
    /// </summary>
    /// <param name="choiceContext">The current choice context.</param>
    /// <param name="cardPlay">The current cardplay.</param>
    /// <param name="pip">The pip being played.</param>
    public Task ModifyPipOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay, Pip pip);
}