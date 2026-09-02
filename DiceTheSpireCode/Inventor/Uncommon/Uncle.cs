using DiceTheSpire.DiceTheSpireCode.Common.Powers;
using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Uncommon;


public class Uncle() : TheInventorCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
{
    public override string GetScrapId => nameof(BigBomb);

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new IntVar("Delay", 5), new DamageVar(90, DamageProps.nonCardHpLoss)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await UnclePower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars["Delay"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Delay"].UpgradeValueBy(-1);
    }
}