using DiceTheSpire.Shared.Cards;
using DiceTheSpire.Shared.Listeners;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Localization.Formatters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Thief.Uncommon;

public class Slimepick() : TheThiefCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self), IModifyPipOnPlayListener
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new PowerVar<PoisonPower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Pip>(), HoverTipFactory.FromPower<PoisonPower>()];

    private List<CardModel> _addedPips = [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        IEnumerable<CardModel> pips = await Pip.CreateInHandAsync(Owner, DynamicVars.Cards.IntValue, CombatState);
        List<CardModel> pipList = [.. pips];
        _addedPips = pipList;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars.Poison.UpgradeValueBy(1);
    }

    public bool ShouldModify(Pip pip)
    {
        return _addedPips.Contains(pip);
    }

    public LocString? PipDescription => null;
    public IEnumerable<IHoverTip> PipHoverTips => [HoverTipFactory.FromPower<PoisonPower>()];
    public Task ModifyPipOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay, Pip pip)
    {
        return Task.CompletedTask;
    }

    public decimal ModifyPoisonAppliedByPip(Pip pip, decimal currentPoison)
    {
        if (!ShouldModify(pip))
        {
            return currentPoison;
        }

        return currentPoison + DynamicVars.Poison.BaseValue;
    }
}