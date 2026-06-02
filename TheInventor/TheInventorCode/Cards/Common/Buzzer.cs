using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Commands;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Common;


public class Buzzer() : TheInventorCard(-1, CardType.Skill, CardRarity.Common, TargetType.AllEnemies)
{
    public override string GetScrapId => nameof(ShortCircuit);

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VulnerablePower>(1)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];

    public LocString JinxDesc => new("cards", Id.Entry + ".jinxDescription");
    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        await JinxCmd.JinxAsync(choiceContext, Owner.Creature, 1, false, JinxDesc, StartOfNextTurnAsync, Owner.Creature, this);
    }

    private async Task StartOfNextTurnAsync(PlayerChoiceContext choiceContext, Creature target)
    {
        if (CombatState is null)
        {
            return;
        }
        await PowerCmd.Apply<VulnerablePower>(choiceContext, CombatState.Enemies, DynamicVars.Power<VulnerablePower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }
}