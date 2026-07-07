using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Rare;

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
            TemporaryGadgetPower? power = await PowerCmd.Apply<TemporaryGadgetPower>(choiceContext, target.Creature, 1, Owner.Creature, this);
            if (power is null)
            {
                return;
            }

            await power.RandomizeThis();
            await power.LinkedGadgetModel.OnRechargeAsync(choiceContext, target);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}