using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Rare;


public class Flamethrower() : TheInventorCard(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    public override string GetScrapId => nameof(Blowtorch);

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(18, DamageProps.card)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState)
            .WithValueProp(DynamicVars.Damage.Props)
            .BeforeDamage(() =>
            {
                foreach (Creature c in CombatState.Enemies.ToArray())
                {
                    NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(c));
                }

                return Task.CompletedTask;
            })
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(2);
        DynamicVars.Damage.UpgradeValueBy(30);
    }
}