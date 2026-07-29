using BaseLib.Extensions;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class Solenoid() : TheInventorCard(3, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
{
    public override string GetScrapId => nameof(BattleWrench);

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(6), new PowerVar<ShockPower>(6)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<ShockPower>(DynamicVars.Power<ShockPower>().IntValue)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }
        foreach (Player p in CombatState.Players)
        {
            await CardPileCmd.DrawWithoutBlockingOnOtherPlayers(choiceContext, DynamicVars.Cards.IntValue, p);
            if (p == Owner)
            {
                await ShockPower.ApplyAsync(choiceContext, p.Creature, DynamicVars.Power<ShockPower>().IntValue, p.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(3);
    }
}