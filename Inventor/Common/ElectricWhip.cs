using BaseLib.Extensions;
using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Common;

public class ElectricWhip() : TheInventorCard(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{ 
    public override string GetScrapId => nameof(ShortCircuit);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, DamageProps.card), new ShockVar(2)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(ShockModel.Shock)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        //ArgumentNullException.ThrowIfNull(cardPlay.Target);

        if (CombatState is null || RunState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState)
            .WithValueProp(DynamicVars.Damage.Props)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        await InventorHelperFunctions.ShockRandomAsync(choiceContext, Owner, RunState.Rng.CombatCardSelection, DynamicVars.Shock.IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Shock.UpgradeValueBy(-1);
    }
}