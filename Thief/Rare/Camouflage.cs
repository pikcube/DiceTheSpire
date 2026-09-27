using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Thief.Rare;

public class Camouflage() : TheThiefCard(0, CardType.Power, CardRarity.Rare, TargetType.Self), ICountdown
{
    public int MaxCount
    {
        get;
        set;
    } = 3;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ReducePower>(3)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ReducePower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<ReducePower>(choiceContext, Owner.Creature, DynamicVars["ReducePower"].BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ReducePower"].UpgradeValueBy(1);
    }
}