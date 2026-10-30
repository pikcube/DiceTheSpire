using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Rare;

public class Javelin() : TheWarriorCard(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    private List<CardModel> CardsInAutoplay { get; set; } = [];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), .. RangeVars.Make(3, 3)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToPull, 0, 2);
        IEnumerable<CardModel> results = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(Owner), Owner, cardSelectorPrefs);
        foreach (CardModel card in results)
        {
            CardsInAutoplay.Add(card);
            await CardCmd.AutoPlay(choiceContext, card, null);
            CardsInAutoplay.Remove(card);
        }
    }

    public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay,
        ResourceInfo resources,
        CardLocation cardLocation)
    {
        return CardsInAutoplay.Contains(card) && isAutoPlay == true && cardLocation.pileType == PileType.Discard ? new CardLocation(cardLocation.player, PileType.Draw, CardPilePosition.Random) : cardLocation;
    }
    protected override void OnUpgrade()
    {
        DynamicVars.MinRange.UpgradeValueBy(-3);
    }

    protected override void AfterCloned()
    {
        base.AfterCloned();
        CardsInAutoplay = [];
    }
}