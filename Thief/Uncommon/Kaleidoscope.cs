using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Thief.Uncommon;

public class Kaleidoscope() : TheThiefCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardSelectorPrefs prefs = new(CardSelectorPrefs.TransformSelectionPrompt, 0, DynamicVars.Cards.IntValue);

        CardModel[] selectedCards = [.. await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)];

        List<CardTransformation> transforms = [];

        List<CardPoolModel> pools = [.. Owner.UnlockState.CharacterCardPools];
        if (pools.Count > 1)
        {
            pools.Remove(Owner.Character.CardPool);
        }

        IEnumerable<CardModel> results = CardFactory.GetDistinctForCombat(Owner, pools.SelectMany(p => p.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)), selectedCards.Length, Owner.RunState.Rng.CombatCardGeneration);

        foreach ((CardModel o, CardModel r) in selectedCards.Zip(results))
        {
            transforms.Add(new CardTransformation(o, r));
        }

        await CardCmd.Transform(transforms, null);


    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}