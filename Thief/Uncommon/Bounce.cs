using BaseLib.Extensions;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Thief.Uncommon;


public class Bounce() : TheThiefCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self), ICountdown
{
    public int MaxCount
    {
        get;
        set;
    } = 2;


    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BouncePower>(1M)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BouncePower>(choiceContext, Owner.Creature, DynamicVars.Power<BouncePower>().BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.UpgradeCountdownBy(-1);
    }
}