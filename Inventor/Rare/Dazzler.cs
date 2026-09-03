using DiceTheSpire.Common.Extensions;
using DiceTheSpire.Common.Utility;
using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Keywords;

namespace DiceTheSpire.Inventor.Rare;

public class Dazzler() : TheInventorCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(Stardust);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips =>
    [
        HoverTipFactory.Static(BetterStaticHoverTips.Inspect, DynamicVars.Cards), HoverTipFactory.FromKeyword(BlinkModel.Blink)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (RunState is null || CombatState is null)
        {
            return;
        }
        int count = await Owner.InspectAsync(choiceContext, DynamicVars.Cards.IntValue);

        for (int n = 0; n < count; ++n)
        {
            foreach (Creature c in CombatState.Enemies)
            {
                await InventorHelperFunctions.ApplyRandomDebuffAsync(choiceContext, RunState, c, Owner.Creature, this);
            }
        }
    }


    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(2);
    }
}