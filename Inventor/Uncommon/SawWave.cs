using BaseLib.Extensions;
using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Inventor.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Uncommon;

public class SawWave() : TheInventorCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(Stardust);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SawWavePower>(1)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<DexterityPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (RunState == null || CombatState is null)
        {
            return;
        }
        await SawWavePower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Power<SawWavePower>().IntValue, Owner.Creature, this);

        await DiceyHelperFunctions.ApplyRandomDebuffAsync(choiceContext, RunState, Owner.Creature, Owner.Creature, this);
        await DiceyHelperFunctions.ApplyRandomDebuffAsync(choiceContext, RunState, Owner.Creature, Owner.Creature, this);
        foreach (Creature creature in CombatState.HittableEnemies)
        {
            await DiceyHelperFunctions.ApplyRandomDebuffAsync(choiceContext, RunState, creature, Owner.Creature, this);
            await DiceyHelperFunctions.ApplyRandomDebuffAsync(choiceContext, RunState, creature, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
    }
}