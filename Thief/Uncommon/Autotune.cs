using DiceTheSpire.Common.Enchantments;
using DiceTheSpire.Thief.Token;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Thief.Uncommon;

public class Autotune() : TheThiefCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<Starpick>(IsUpgraded), HoverTipFactory.FromCard<RollTheBones>(IsUpgraded),
        HoverTipFactory.FromCard<CrossedWire>(IsUpgraded), ..HoverTipFactory.FromEnchantment<Stellar>(),
        HoverTipFactory.Static(StaticHoverTip.SummonStatic), HoverTipFactory.FromPower<DoomPower>(),
        HoverTipFactory.Static(StaticHoverTip.Channeling), HoverTipFactory.FromOrb<PlasmaOrb>()
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        CardModel[] cardChoice =
        [
            CombatState.CreateCard(ModelDb.Card<Starpick>(), Owner),
            CombatState.CreateCard(ModelDb.Card<RollTheBones>(), Owner),
            CombatState.CreateCard(ModelDb.Card<CrossedWire>(), Owner)
        ];
        if (IsUpgraded)
        {
            foreach (CardModel card in cardChoice)
            {
                CardCmd.Upgrade(card);
            }
        }
        CardModel? choice = await CardSelectCmd.FromChooseACardScreen(choiceContext, cardChoice, Owner);

        if (choice is null)
        {
            return;
        }

        choice.SetToFreeThisTurn();
        await CardPileCmd.AddGeneratedCardToCombat(choice, PileType.Hand, Owner);
    }
}