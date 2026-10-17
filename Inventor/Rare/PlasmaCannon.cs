using BaseLib.Extensions;
using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Inventor.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Inventor.Rare;

public class PlasmaCannon() : TheInventorCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(BattleWrench);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PlasmaCannonPower>(1)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Scrap), HoverTipFactory.Static(BetterStaticHoverTips.Gadget)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel[] cards =
        [
            .. Owner.PlayerCombatState?.DrawPile.Cards.Where(c => c.Type is CardType.Power)
                .TakeRandom(2, Owner.RunState.Rng.CombatCardSelection) ?? []
        ];

        await PlasmaCannonPower.ApplyAsync(choiceContext, Owner, DynamicVars.Power<PlasmaCannonPower>().IntValue, Owner, this);

        foreach (CardModel card in cards)
        {
            await CardPileCmd.Add(card, PileType.Play);
        }

        foreach (CardModel card in cards)
        {
            await CardCmd.AutoPlay(choiceContext, card, null);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}