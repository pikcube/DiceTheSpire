using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Uncommon;


public class GiantSpatula() : TheInventorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override string GetScrapId => nameof(Hook);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel[] cards = [.. PileType.Draw.GetPile(Owner).Cards];

        foreach (CardModel card in cards)
        {
            await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top);
        }

        if (Owner.HasPower<FlippedDrawPilePower>())
        {
            await PowerCmd.Remove<FlippedDrawPilePower>(Owner.Creature);
        }
        else
        {
            await FlippedDrawPilePower.ApplyAsync(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}