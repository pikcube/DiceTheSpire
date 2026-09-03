using BaseLib.Extensions;
using DiceTheSpire.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Common;

public class Whisper() : TheInventorCard(-1, CardType.Skill, CardRarity.Common, TargetType.AllEnemies)
{
    public override string GetScrapId => nameof(Burrower);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(1)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<WeakPower>()];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        if (CombatState?.Enemies is null)
        {
            return;
        }

        await WeakPower.ApplyAsync(choiceContext, CombatState.Enemies, DynamicVars.Power<WeakPower>().IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<WeakPower>().UpgradeValueBy(1);
    }
}