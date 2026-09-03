using DiceTheSpire.Common.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Thief.Common;

  public class Hookshot() : TheThiefCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
  {
      protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9M, ValueProp.Move), new PowerVar<HookshotPower>(1M)];

      protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
      {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        await PowerCmd.Apply<HookshotPower>(choiceContext, Owner.Creature, DynamicVars["HookshotPower"].BaseValue,
            Owner.Creature, this);
      }

      protected override void OnUpgrade()
      {
          DynamicVars.Damage.UpgradeValueBy(3M);
      }
  }