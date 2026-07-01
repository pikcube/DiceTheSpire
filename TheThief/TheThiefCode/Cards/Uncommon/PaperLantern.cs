using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheThief.TheThiefCode.Powers;

namespace TheThief.TheThiefCode.Cards.Uncommon;

public class PaperLantern() : TheThiefCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new EnergyVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Pip>(), HoverTipFactory.ForEnergy(this)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<PipNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Cards.BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars.Energy.UpgradeValueBy(1);
    }
}