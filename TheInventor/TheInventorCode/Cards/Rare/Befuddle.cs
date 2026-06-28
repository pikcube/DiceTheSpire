using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes.Cards;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Rare;


public class Befuddle() : TheInventorCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override string GetScrapId => nameof(Hook);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardSelectorPrefs cardSelectorPrefs = new(new LocString("card_selection", "TO_BLINK"), 1, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, cardSelectorPrefs, null, this)).SingleOrDefault();
        if (card is null)
        {
            return;
        }

        await card.BlinkAsync(choiceContext);

        BefuddlePower? power = await BefuddlePower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Cards.EnchantedValue, Owner.Creature, this);

        power?.SetCards(card);

    }
}