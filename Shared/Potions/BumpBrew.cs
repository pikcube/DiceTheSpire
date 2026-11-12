using BaseLib.Extensions;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Code.Extensions;

namespace DiceTheSpire.Shared.Potions;
public class BumpBrew : TheWarriorPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(6)];
    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Bump)];
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        target ??= Owner.Creature;
        CardSelectorPrefs cardSelectorPrefs = new(DiceySelection.ToBump, 0, DynamicVars.Cards.IntValue);
        CardModel[] cards = [.. await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)];
        foreach (CardModel card in cards)
        {
            await card.BumpAsync(choiceContext);
        }
    }
}