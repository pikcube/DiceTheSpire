using BaseLib.Extensions;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Powers;

namespace DiceTheSpire.Warrior.Rare;

public class Shriek() : TheWarriorCard(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ShriekStrengthDownPower>(99M), new PowerVar<BrokenMirrorPower>(1M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<CursedPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsUpgraded)
        {
            if (CombatState is null)
                return;
            await PowerCmd.Apply<ShriekStrengthDownPower>(choiceContext, CombatState.Enemies, DynamicVars.Power<ShriekStrengthDownPower>().IntValue, Owner.Creature, cardPlay.Card);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            await PowerCmd.Apply<ShriekStrengthDownPower>(choiceContext, cardPlay.Target, DynamicVars.Power<ShriekStrengthDownPower>().IntValue, Owner.Creature, cardPlay.Card);
        }
        await PowerCmd.Apply<BrokenMirrorPower>(choiceContext, Owner.Creature, DynamicVars.Power<BrokenMirrorPower>().IntValue, Owner.Creature, this);


    }

    public override TargetType TargetType => IsUpgraded ? TargetType.AllEnemies : TargetType.AnyEnemy;
}

