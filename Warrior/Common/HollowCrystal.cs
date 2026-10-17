using BaseLib.Cards.Variables;
using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Common;
public class HollowCrystal() : TheWarriorCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy), ICrystalCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, DamageProps.card), .. MakeCalculatedVar("HitCount", 1, BonusFunc)];

    private static decimal BonusFunc(CardModel card, Creature? target)
    {
        if (card.Owner.PlayerCombatState is null)
        {
            return 1;
        }

        return (decimal)card.Owner.PlayerCombatState.Hand.Cards.Count(c => c is ICrystalCard) - 1;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        CustomCalculatedVar hitCountVar = (CustomCalculatedVar)DynamicVars["HitCount"];
        int count = (int)hitCountVar.Calculate(null) + 1;

        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .FromCard(this, cardPlay)
            .WithHitCount(count) 
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}

