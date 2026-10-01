using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Thief.Common;

public class Cloak() : TheThiefCard(0, CardType.Skill, CardRarity.Common, TargetType.Self), ICountdown
{
    public int BaseCount
    {
        get;
        set;
    } = 1;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DexterityPower>(3M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DexterityPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<CloakPower>(choiceContext, Owner.Creature, DynamicVars.Dexterity.EnchantedValue, Owner.Creature, this);

    }

    protected override void OnUpgrade()
    {
        DynamicVars.Dexterity.UpgradeValueBy(2);
    }
}