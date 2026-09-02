using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using DiceTheSpire.DiceTheSpireCode.Powers;
using DiceTheSpire.DiceTheSpireCode.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Cards.Rare;

public class Wikipedia() : TheInventorCard(4, CardType.Power, CardRarity.Rare, TargetType.AllAllies)
{
    public override string GetScrapId => nameof(SharedInterest);
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.Static(InventorStaticHoverTips.TemporaryGadget)];
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }
        foreach (Player target in CombatState.Players.Where(p => p != Owner))
        {
            BranchingPlayerChoiceContext bpcc = new(LocalContext.NetId ?? 0, GameActionType.CombatPlayPhaseOnly, choiceContext);
            Task task = GainGadgetAsync(bpcc, target);
            await bpcc.AssignTaskAndWaitForPauseOrCompletion(task);
        }
    }

    private async Task GainGadgetAsync(BranchingPlayerChoiceContext choiceContext, Player target)
    {
        TemporaryGadgetPower? power = await PowerCmd.Apply<TemporaryGadgetPower>(choiceContext, target.Creature, 1, Owner.Creature, this);
        if (power is null)
        {
            return;
        }

        await power.RandomizeThisAsync();
        await power.LinkedGadgetModel.OnRechargeAsync(choiceContext, target);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}