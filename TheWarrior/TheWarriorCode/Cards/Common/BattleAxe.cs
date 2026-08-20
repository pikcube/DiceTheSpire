using DiceTheSpireCore.DiceTheSpireCoreCode.DynamicVars;
using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Common;

public class BattleAxe() : TheWarriorCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, DamageProps.card), new RepeatVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        int extraHits = Owner.Creature.GetPower<AxeMasteryPower>()?.Amount ?? 0;
            
        int hitCount = 2 + extraHits;

        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .WithHitCount(hitCount)
            .FromCard(this, cardPlay)
            .WithHitFx(VfxCmd.slashPath)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        await PowerCmd.Apply<AxeMasteryPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);

        if (IsUpgraded)
        {
            await PowerCmd.Apply<AxeMasteryPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        }
    }

  //"THEWARRIOR-BATTLE_AXE.description": "Deal {Damage:diff()} damage twice.\nHits once more for each [gold]Battle Axe[/gold] played this combat.",
  //"THEWARRIOR-BATTLE_AXE.title": "Battle Axe",

}