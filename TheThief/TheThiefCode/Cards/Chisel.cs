using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheThief.TheThiefCode.Cards;

  
public class Chisel() : TheThiefCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [new CardHoverTip(ModelDb.Card<Splinter>())];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }
        CardSelectorPrefs prefs = new(CardSelectorPrefs.EnchantSelectionPrompt, 1);
        CardModel? original = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
        if (original == null)
        {
            return;
        }
        original.EnergyCost.AddThisTurnOrUntilPlayed(-1);

        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Splinter>(Owner), PileType.Hand, Owner);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}