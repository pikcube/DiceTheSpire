using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Common.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.DiceTheSpireCode.Warrior.Uncommon;
public class Nightstick() : TheWarriorCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, DamageProps.card), new PowerVar<NightstickStrengthDownPower>(4)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()]; 

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        
        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);


        await PowerCmd.Apply<NightstickStrengthDownPower>(choiceContext, cardPlay.Target, DynamicVars.Power<NightstickStrengthDownPower>().IntValue,
            Owner.Creature, cardPlay.Card);


    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<NightstickStrengthDownPower>().UpgradeValueBy(1);
    }
}