using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace TheThief.TheThiefCode.Cards.Uncommon;

  
public class Chisel() : TheThiefCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [new CardHoverTip(ModelDb.Card<Pip>())];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }
        CardSelectorPrefs prefs = new(new LocString("card_selection", "TO_MODIFY_COST"), 1);
        CardModel? original = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
        if (original == null)
        {
            return;
        }
        if (original.EnergyCost.Canonical > 1)
        {
            original.EnergyCost.AddThisTurnOrUntilPlayed(-1);
        }

        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Pip>(Owner), PileType.Hand, Owner);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}