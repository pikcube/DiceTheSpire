using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Warrior.Uncommon;

public class FlamingSword() : TheWarriorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    private int TimesPlayed { get; set; }
    protected override IEnumerable<DynamicVar> CanonicalVars => [.. MakeCalculatedVar("ScalingStrengthPower", 2, Bonus)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    private static decimal Bonus(CardModel card, Creature? arg2)
    {
        if (card is not FlamingSword fs)
        {
            //What? Why? How?
            return 69420;
        }

        if (card.IsUpgraded)
        {
            return 3 * fs.TimesPlayed;
        }
        return 2 * fs.TimesPlayed;
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars["ScalingStrengthPower"].IntValue, Owner.Creature, this);

        TimesPlayed++;
    }
}