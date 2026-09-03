using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Inventor.Rare;

public class Fireworks() : TheInventorCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override string GetScrapId => nameof(Burrower);

    protected override IEnumerable<DynamicVar> CanonicalVars => [..MakeCalculatedDamage(0, Bonus, 4)];

    private static decimal Bonus(CardModel card, Creature? target)
    {
        if (card is not Fireworks bd || bd.CombatState is null)
        {
            return 1;
        }

        return bd.CombatState.Creatures.SelectMany(c => c.Powers).Count(p => p.Type == PowerType.Debuff);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(1);
    }
}