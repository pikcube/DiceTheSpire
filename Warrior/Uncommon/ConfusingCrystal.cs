using BaseLib.Extensions;
using DiceTheSpire.Inventor.Powers;
using DiceTheSpire.Shared.Commands;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Keywords;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace DiceTheSpire.Warrior.Uncommon;

public class ConfusingCrystal() : TheWarriorCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self), ICrystalCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Reroll)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Ethereal];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ConfusingCrystalPower>(1M)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

    }
    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card == this)
        {
            await PowerCmd.Apply<ConfusingCrystalPower>(choiceContext, Owner.Creature, DynamicVars.Power<ConfusingCrystalPower>().IntValue, Owner.Creature, this);
        }
    }
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

}