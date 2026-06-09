using DiceTheSpireCore.DiceTheSpireCoreCode.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheThief.TheThiefCode.Cards.Uncommon;

public class SpikySneeze() : TheThiefCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(9, ValueProp.Move), new IntVar("CountdownVar", 2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath).Execute(choiceContext);

        CardModel? card = Owner.RunState.Rng.CombatCardSelection.NextItem(PileType.Hand.GetPile(Owner).Cards.Where(c => c is ICountdown));
        if (card is ICountdown countdownCard)
        {
            await countdownCard.DecrementCountAsync(2);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}