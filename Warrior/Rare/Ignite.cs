using BaseLib.Extensions;
using DiceTheSpire.Shared.DynamicVars;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using DiceTheSpire.Warrior;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace DiceTheSpire.Inventor.Uncommon;
public class Ignite() : TheWarriorCard(2, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FuryPower>(1M), .. RangeVars.Make(2, 2)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FuryPower>(DynamicVars.Power<FuryPower>().IntValue)];
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        foreach (Player p in CombatState.Players) //.Where(p => p != Owner)
        {
            await PowerCmd.Apply<FuryPower>(choiceContext, p.Creature, 1M, Owner.Creature, this);
        }

    }
    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}