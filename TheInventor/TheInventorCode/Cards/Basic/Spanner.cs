using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Cards.Ancient;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Basic;

public class Spanner() : TheInventorCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self), ITranscendenceCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await base.OnPlay(choiceContext, cardPlay);
        await CardCmd.DiscardAndDraw(choiceContext, PileType.Hand.GetPile(Owner).Cards.TakeRandom(2, Owner.RunState.Rng.CombatCardSelection), DynamicVars.Cards.IntValue);
    }

    public override string OnScrap()
    {
        return nameof(BattleWrench);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    public CardModel GetTranscendenceTransformedCard() => SteelWrench.CreateWithoutOwner();
}