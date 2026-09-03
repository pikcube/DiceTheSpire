using BaseLib.Utils;
using DiceTheSpire.DiceTheSpireCode.Common.Cards;
using DiceTheSpire.DiceTheSpireCode.Common.Enchantments;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace DiceTheSpire.DiceTheSpireCode.Thief.Token;

[Pool(typeof(TokenCardPool))]
public class Starpick() : TheThiefCard(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Pip>(), ..HoverTipFactory.FromEnchantment<Stellar>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        foreach (CardModel card in await Pip.CreateInHandAsync(Owner, DynamicVars.Cards.IntValue, CombatState))
        {
            CardCmd.Enchant<Stellar>(card, 1);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}