using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;
using Pikcube.Common.Keywords;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class DiceDetonator() : TheInventorCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override string GetScrapId => nameof(Catapult);
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, DamageProps.card)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.PlayerCombatState is null || CombatState is null)
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        List<CardModel> blinkCards = [.. Owner.PlayerCombatState.AllCards.Where(c => c.Owner == Owner && c.Keywords.Contains(BlinkedModel.Blinked))];

        foreach (CardModel c in blinkCards)
        {
            CardPileAddResult? result = await CardCmd.Transform(c, Dazed.Create(Owner, CombatState), CardPreviewStyle.GridLayout);
            result?.cardAdded.AddPurpleKeyword(BlinkedModel.Blinked);
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitCount(blinkCards.Count)
            .WithHitFx(VfxCmd.rockShatterPath)
            .Execute(choiceContext);

        foreach (CardModel card in blinkCards)
        {
            card.RemoveKeyword(BlinkedModel.Blinked);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}