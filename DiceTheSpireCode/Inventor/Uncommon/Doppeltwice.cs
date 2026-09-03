using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Uncommon;

public class Doppeltwice() : TheInventorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(MagicDice);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        await EnergyNextTurnPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Energy.IntValue,
            Owner.Creature, this);
    }


    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
        AddKeyword(BlinkModel.Blink);
    }
}