using DiceTheSpire.Common.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Warrior.Common;

public class ThickSkin() : TheWarriorCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new BlockVar(4M, BlockProps.card)];
    public override bool GainsBlock => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<RollAgain>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        if (CombatState is null)
        {
            return;
        }
        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
        if (IsUpgraded)
        {
            await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner);
        }

        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }
    protected override void OnUpgrade()
    {
        base.OnUpgrade();
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars.Block.UpgradeValueBy(1);
    }
}