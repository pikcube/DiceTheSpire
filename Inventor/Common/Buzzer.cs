using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Common;


public class Buzzer() : TheInventorCard(-1, CardType.Skill, CardRarity.Common, TargetType.AllEnemies)
{ 
    public override string GetScrapId => nameof(ShortCircuit);

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VulnerablePower>(1)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];
    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        if (CombatState is null)
        {
            return;
        }

        await NextTurnVulnerablePower.ApplyAsync(choiceContext, CombatState.Enemies, DynamicVars.Vulnerable.IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }
}