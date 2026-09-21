using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Common;

public class Nudgeblade() : TheWarriorCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new DamageVar(6, DamageProps.card)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Nudge)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);

        if (CombatState is null || Owner.PlayerCombatState is null)
        {
            return;
        }

        if (IsUpgraded)
        {
            CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToNudge, 0, 2);
            IEnumerable<CardModel> results = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner), Owner, cardSelectorPrefs);
            foreach (CardModel card in results)
            {
                await NudgeCmd.NudgeAsync(card, NudgeDuration.UntilPlayed);
            }
        }
        else
        {
            CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToNudge, 0, 2);
            IEnumerable<CardModel> results = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(Owner), Owner, cardSelectorPrefs);
            foreach (CardModel card in results)
            {
                await NudgeCmd.NudgeAsync(card, NudgeDuration.UntilPlayed);
            }
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}