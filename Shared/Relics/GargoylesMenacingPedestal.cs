using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Powers;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Shared.Relics;

[UsedImplicitly]
public class GargoylesMenacingPedestal : TheWarriorRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
    {
        if (shuffler != Owner)
            return;
        CardModel[] cards =
            [
                    ..PileType.Draw.GetPile(Owner).Cards
                    .Where(c => !c.Keywords.Contains(CardKeyword.Unplayable) && !c.EnergyCost.CostsX)
                    .TakeRandom((int)DynamicVars.Cards.BaseValue, Owner.RunState.Rng.CombatCardSelection)
            ];
        foreach (CardModel card in cards)
        {
            await NudgeCmd.NudgeAsync(card, NudgeDuration.UntilPlayed);            
            //card.EnergyCost.SetThisTurnOrUntilPlayed(card.EnergyCost.GetAmountToSpend() - 1);
        }


    }



}