using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Uncommon;

public class Solenoid() : TheInventorCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
{
    public override string GetScrapId => nameof(BattleWrench);

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(6)];

    private int ShockValForTip => IsUpgraded ? 1 : -2;

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<ShockPower>(ShockValForTip)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }
        foreach (Player p in CombatState.Players)
        {
            if (p == Owner)
            {
                int cardsToShock = IsUpgraded ? 1 : Owner.PlayerCombatState?.Hand.Cards.Count ?? 10;
                await ShockPower.ApplyAsync(choiceContext, p.Creature, cardsToShock, p.Creature, this);
            }
            else
            {
                await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.EnchantedValue, p);
            }
        }
    }
}