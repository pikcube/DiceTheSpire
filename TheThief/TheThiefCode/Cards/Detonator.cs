using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheThief.TheThiefCode.Cards;

  
public class Detonator() : TheThiefCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        int damage = 0;
        if (cardPlay.Target.HasPower<PoisonPower>())
        {
            damage = cardPlay.Target.GetPowerAmount<PoisonPower>();
        }

        await DamageCmd.Attack(damage).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}