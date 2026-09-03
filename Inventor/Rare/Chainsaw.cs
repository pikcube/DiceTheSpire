using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Inventor.Rare;

public class Chainsaw() : TheInventorCard(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{ 
    protected override IEnumerable<DynamicVar> CanonicalVars => [..MakeCalculatedDamage(19, Bonus, 26)];

    private static decimal Bonus(CardModel card, Creature? target)
    { 
        return target?.HasPower<MinionPower>() is true ? 1 : 0;
    }

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<MinionPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(8);
    }

    public override string GetScrapId => nameof(DialUpSounds);
}