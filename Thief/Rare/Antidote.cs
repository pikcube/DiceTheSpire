using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Thief.Rare;

public class Antidote() : TheThiefCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

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

        var strength = Owner.Creature.GetPowerAmount<StrengthPower>();
        var dexterity = Owner.Creature.GetPowerAmount<DexterityPower>();
        if (strength < 0)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, -strength, Owner.Creature, this);
        }

        if (dexterity < 0)
        {
            await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, -dexterity, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}