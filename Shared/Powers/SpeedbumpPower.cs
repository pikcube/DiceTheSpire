using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Powers;
public class SpeedbumpPower : DiceTheSpirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private int Counter { get; set; }
    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
    ICombatState combatState)
    {
        if (side == Owner.Side)
        {
            Counter = 0;
        }

        return Task.CompletedTask;
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        Counter = 0;
        return Task.CompletedTask;
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Bump)];

    public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (Owner.Player is null || Counter < 0 || Counter >= Amount) //card.MaxUpgradeLevel == 0 || Counter < 0 || Counter >= 1
        {
            return;
        }

        SpeedbumpPower speedbumpPower = this;
        speedbumpPower.Flash();

        if (Counter <= Amount)
        {
            await card.BumpAsync(choiceContext);
        }
        ++Counter;

        //CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToBump, 0, Amount);
        //CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, Owner.Player, cardSelectorPrefs, null, this)];
        //foreach (CardModel c in cards)
        //{
        //    await c.BumpAsync(choiceContext);

        //    if (Counter == 0)
        //    {
        //        await c.BumpAsync(choiceContext);
        //    }
        //    ++Counter;
    }
}