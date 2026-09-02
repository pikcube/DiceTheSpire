using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.DiceTheSpireCode.Thief.Uncommon;

public class MoodChange() : TheThiefCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<DexterityPower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        var strength = Owner.Creature.GetPower<StrengthPower>()?.Amount ?? 0;
        var dexterity = Owner.Creature.GetPower<DexterityPower>()?.Amount ?? 0;

        await PowerCmd.Remove<StrengthPower>(Owner.Creature);
        await PowerCmd.Remove<DexterityPower>(Owner.Creature);

        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, dexterity, Owner.Creature, this);
        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, strength, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}