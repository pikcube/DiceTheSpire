using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Common;


public class PuppyPaws() : TheInventorCard(2, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override string GetScrapId => nameof(MagicDice);
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(11, BlockProps.card), new EnergyVar(2)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await EnergyNextTurnPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Energy.IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }
}