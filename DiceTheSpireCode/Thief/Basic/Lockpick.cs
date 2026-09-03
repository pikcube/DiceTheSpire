using BaseLib.Abstracts;
using DiceTheSpire.DiceTheSpireCode.Common.Cards;
using DiceTheSpire.DiceTheSpireCode.Thief.Ancient;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.DiceTheSpireCode.Thief.Basic;

public class Lockpick() : TheThiefCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self), ITranscendenceCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [new CardHoverTip(ModelDb.Card<Pip>())];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        await Pip.CreateInHandAsync(Owner, DynamicVars.Cards.IntValue, CombatState);
        await Cmd.Wait(0.5f);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    public CardModel GetTranscendenceTransformedCard() => MasterKey.Create();
}