using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Listeners;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Common;


[UsedImplicitly]
public class Resistor() : TheInventorCard(2, CardType.Skill, CardRarity.Common, TargetType.Self), IOnShockListener
{
    public override string GetScrapId => nameof(WallOfIce);
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6, BlockProps.card)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromKeyword(ShockModel.Shock)];

    private Player? SafeOwner => IsCanonical ? null : Owner;

    public override TargetType TargetType => SafeOwner?.RunState.CardMultiplayerConstraint == CardMultiplayerConstraint.MultiplayerOnly ? TargetType.AnyPlayer : TargetType.Self;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await CreatureCmd.GainBlock(cardPlay.Target ?? Owner.Creature, DynamicVars.Block, cardPlay);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(1);
    }

    public async Task AfterCardShockedAsync(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card != this)
        {
            return;
        }

        await card.UnshockAsync();
    }
}