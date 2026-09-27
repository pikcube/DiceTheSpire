using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Thief.Rare;

public class Blight() : TheThiefCard(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy), ICountdown
{
    public int BaseCount
    {
        get;
        set;
    } = 4;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PoisonPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        if (cardPlay.Target.HasPower<PoisonPower>())
        {
            await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, cardPlay.Target.GetPowerAmount<PoisonPower>(),
                Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        this.UpgradeCountdownBy(-1);
    }
}