using BaseLib.Utils;
using DiceTheSpire.Shared.Cards;
using DiceTheSpire.Shared.Listeners;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using Pikcube.Common.Code.Extensions;

namespace DiceTheSpire.Thief.Token;

[Pool(typeof(TokenCardPool))]
public class Starpick() : TheThiefCard(1, CardType.Skill, CardRarity.Token, TargetType.Self), IModifyPipOnPlayListener
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new StarsVar(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Pip>()];

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
    }

    public bool ShouldModify(Pip pip)
    {
        return _addedPips.Contains(pip);
    }

    public LocString PipDescription => new LocString("cards", Id.Entry + ".pipDescription").WithDynamicVars(DynamicVars);

    public IEnumerable<IHoverTip> PipHoverTips => [];
    public async Task ModifyPipOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay, Pip pip)
    {
        if (!ShouldModify(pip))
        {
            return;
        }

        await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
    }

    public decimal ModifyPoisonAppliedByPip(Pip pip, decimal currentPoison)
    {
        return currentPoison;
    }
}