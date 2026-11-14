using DiceTheSpire.Shared.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Thief.Rare;

public class Spare1() : TheThiefCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override string Title => IsUpgraded ? UpgradedTitleLocString.GetFormattedText() : TitleLocString.GetFormattedText();
    public LocString UpgradedTitleLocString
    {
        get
        {
            field ??= new LocString("cards", Id.Entry + ".upgradeTitle");
            return field;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new EnergyVar(1), new IntVar("Pips", 1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Pip>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        await Pip.CreateInHandAsync(Owner, DynamicVars["Pips"].IntValue, CombatState);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars.Energy.UpgradeValueBy(1);
        DynamicVars["Pips"].UpgradeValueBy(1);
    }
}