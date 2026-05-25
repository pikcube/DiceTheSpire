using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Keywords;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Relics;

namespace TheInventor.TheInventorCode.Cards.Uncommon;

public class EmptySlot() : TheInventorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(DefaultGadget);

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BlinkModel.Blink];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        CardModel? card = CardFactory
            .GetDistinctForCombat(Owner, Owner.Character.CardPool
                .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint), 1, Owner.RunState.Rng.CombatCardGeneration)
            .FirstOrDefault();

        if (card is null)
        {
            return;
        }
        card.SetToFreeThisTurn();
        
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override bool ModifyScrap(Gadget gadget, AbstractGadget linkedGadget)
    {
        return true;
    }
}