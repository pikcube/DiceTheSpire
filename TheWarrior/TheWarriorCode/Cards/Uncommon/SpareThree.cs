using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheWarrior.TheWarriorCode.Extensions;

namespace TheWarrior.TheWarriorCode.Cards.Uncommon;
public class SpareThree() : TheWarriorCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self), IRangeCard
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

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(3), new PowerVar<NoDrawPower>(1M), new StringVar("Title")];

    //public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [BetterStaticHoverTips.RangeHoverTip(this)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        await PowerCmd.Apply<NoDrawPower>(choiceContext, Owner.Creature, DynamicVars.Power<NoDrawPower>().BaseValue, Owner.Creature, this);
    }

    public override string CustomPortraitPath => IsUpgraded ? "spare_four.png".BigCardImagePath() : "spare_three.png".BigCardImagePath();

    public int MinimumCost => 0;

    public int MaximumCost => 0;

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }
}

