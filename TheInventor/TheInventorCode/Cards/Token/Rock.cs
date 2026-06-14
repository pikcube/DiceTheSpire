using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Token;

public class Rock() : TheInventorCard(-1, CardType.Attack, CardRarity.Token, TargetType.RandomEnemy)
{
    public override string GetScrapId => nameof(Bonk);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, DamageProps.card)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable, CardKeyword.Ethereal];

    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        if (CombatState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingRandomOpponents(CombatState)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}