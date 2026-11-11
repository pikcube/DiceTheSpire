using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Code.Extensions;

namespace DiceTheSpire.Inventor.Uncommon;


public class DoubleEdge() : TheInventorCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(PowerUp);
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(2), new PowerVar<DexterityPower>(4)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<DexterityPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await StrengthPower.ApplyAsync(choiceContext, Owner.Creature, -DynamicVars.Strength.IntValue,
            Owner.Creature, this);
        await DexterityPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Dexterity.IntValue, Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Strength.UpgradeValueBy(-1);
        DynamicVars.Dexterity.UpgradeValueBy(1);
    }
}