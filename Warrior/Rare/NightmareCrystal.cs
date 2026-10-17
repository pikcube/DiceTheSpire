using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.Rare;
public class NightmareCrystal() : TheWarriorCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self), ICrystalCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new CardsVar(3)];
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToDupe, 1, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)).SingleOrDefault();
        if (card is null)
        {
            return;
        }

        NightmareCrystalPower? power = await NightmareCrystalPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Cards.IntValue, Owner.Creature, this);

        power?.SetCard(card);

    }
    

}


