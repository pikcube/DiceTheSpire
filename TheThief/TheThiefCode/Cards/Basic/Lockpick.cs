using BaseLib.Abstracts;
using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;
using TheThief.TheThiefCode.Cards.Ancient;

namespace TheThief.TheThiefCode.Cards.Basic;

public class Lockpick() : TheThiefCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self), ITranscendenceCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [new CardHoverTip(ModelDb.Card<Pip>())];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        //CombatState might be null according to type annotations, so I added a null check
        if (CombatState is null)
        {
            return;
        }

        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Pip>(Owner), PileType.Hand, Owner);
        await Cmd.Wait(0.5f);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    public CardModel GetTranscendenceTransformedCard() => MasterKey.Create();
}