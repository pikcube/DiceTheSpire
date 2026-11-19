using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Thief.Uncommon;

public class Camouflage() : TheThiefCard(0, CardType.Power, CardRarity.Uncommon, TargetType.Self), ICountdown
{
    public int BaseCount
    {
        get;
        set;
    } = 3;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(3, ValueProp.Unpowered)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await PowerCmd.Apply<CamouflagePower>(choiceContext, Owner.Creature, DynamicVars.Block.BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.UpgradeCountdownBy(-1);
    }
}