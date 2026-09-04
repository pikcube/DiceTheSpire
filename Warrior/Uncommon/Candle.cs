using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Uncommon;

public class Candle() : TheWarriorCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(10), new BlockVar(2, BlockProps.card)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Nudge), HoverTipFactory.FromCard<Burn>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToNudge, 0, DynamicVars.Cards.IntValue);
        CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
        foreach (CardModel card in cards)
        {
            //if (card.CurrentUpgradeLevel > 0)
            //{
                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            //}
            await NudgeCmd.NudgeAsync(card, NudgeDuration.UntilPlayed);
        }
        if (CombatState is null)
        {
            return;
        }
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Burn>(Owner), PileType.Discard, Owner));
        await Cmd.Wait(0.5f);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
    }
}