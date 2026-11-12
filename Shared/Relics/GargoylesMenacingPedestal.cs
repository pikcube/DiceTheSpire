using DiceTheSpire.Shared.Commands;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Shared.Relics;

[UsedImplicitly]
public class GargoylesMenacingPedestal : TheWarriorRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
    {
        if (shuffler != Owner)
        {
            return;
        }

        Flash();
        CardModel[] cards =
        [
            ..PileType.Draw.GetPile(Owner).Cards
                .Where(c => !c.Keywords.Contains(CardKeyword.Unplayable) && !c.EnergyCost.CostsX && c.EnergyCost.GetAmountToSpend() > 0)
                .TakeRandom((int)DynamicVars.Cards.BaseValue, Owner.RunState.Rng.CombatCardSelection)
        ];
        foreach (CardModel card in cards)
        {
            await NudgeCmd.NudgeAsync(card, NudgeDuration.UntilPlayed);            
        }
    }
}