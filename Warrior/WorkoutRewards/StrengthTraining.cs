using BaseLib.Extensions;
using DiceTheSpire.Shared.Cards;
using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Listeners;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;



namespace DiceTheSpire.Warrior.WorkoutRewards;
public class StrengthTraining() : TheWarriorCard(3, CardType.Power, CardRarity.Token, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(2M), new PowerVar<DexterityPower>(2M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<DexterityPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars.Power<StrengthPower>().IntValue, Owner.Creature, this);
        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, DynamicVars.Power<DexterityPower>().IntValue, Owner.Creature, this);

        if (CombatState is null)
        {
            return;
        }
        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<StrengthTraining>(Owner), PileType.Discard, Owner);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Power<StrengthPower>().UpgradeValueBy(1);
        DynamicVars.Power<DexterityPower>().UpgradeValueBy(1);
    }

}
