using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DiceTheSpire.Warrior.Rare;

//  "DICETHESPIRE-CRYSTALIZE.description": "At the start of each turn\ncreate X {IfUpgraded:show:[gold]Upgraded[/gold] |}\"Crystal\" card in [gold]Hand[/gold].",

public class Crystalize() : TheWarriorCard(-1, CardType.Power, CardRarity.Rare, TargetType.Self), ICrystalCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];
    protected override bool HasEnergyCostX => true;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        int xValue = cardPlay.Card.ResolveEnergyXValue();

        if (IsUpgraded)
        {
            await PowerCmd.Apply<CrystalizationPower>(choiceContext, Owner.Creature, xValue + 1, Owner.Creature, this);
        }
        else
        {
            await PowerCmd.Apply<CrystalizationPower>(choiceContext, Owner.Creature, xValue, Owner.Creature, this);
        }

    }
}

