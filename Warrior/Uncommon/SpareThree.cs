using BaseLib.Extensions;
using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Warrior.Uncommon;
public class SpareThree() : TheWarriorCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
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

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(3), new PowerVar<NoDrawPower>(1M), new StringVar("Title"), .. RangeVars.Make(0, 0)];

    //public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        await PowerCmd.Apply<NoDrawPower>(choiceContext, Owner.Creature, DynamicVars.Power<NoDrawPower>().BaseValue, Owner.Creature, this);
    }

    public override string CustomPortraitPath => IsUpgraded ? "spare_four.png".BigCardImagePath() : "spare_three.png".BigCardImagePath();

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }
}

