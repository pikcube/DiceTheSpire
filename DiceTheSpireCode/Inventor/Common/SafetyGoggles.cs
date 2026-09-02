using BaseLib.Extensions;
using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using DiceTheSpireCore.DiceTheSpireCoreCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Common;


public class SafetyGoggles() : TheInventorCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override string GetScrapId => nameof(Protection);
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new BlockVar(7, BlockProps.card),
        new PowerVar<SafetyGogglesPower>(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await SafetyGogglesPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Power<SafetyGogglesPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
}