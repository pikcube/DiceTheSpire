using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Rare;

public class Doppeltwice() : TheInventorCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(MagicDice);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [ShockModel.Shock];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        await EnergyNextTurnPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Energy.IntValue,
            Owner.Creature, this);
    }


    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }
}