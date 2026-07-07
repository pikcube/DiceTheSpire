using DiceTheSpireCore.DiceTheSpireCoreCode;
using DiceTheSpireCore.DiceTheSpireCoreCode.Cards;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWarrior.TheWarriorCode.Relics;
[UsedImplicitly]
public class CombatRoll : TheWarriorRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Reroll), HoverTipFactory.FromCard<RollAgain>(true)];
    public override async Task BeforeHandDraw(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
    {
        if (Owner.Creature.CombatState is null || Owner.Creature.CombatState.RoundNumber != 1)
        {
            return;
        }
        CardModel card = Owner.Creature.CombatState.CreateCard<RollAgain>(Owner);
        if (card is null)
        {
            return;
        }
        CardCmd.Upgrade(card);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        CardModel card2 = Owner.Creature.CombatState.CreateCard<RollAgain>(Owner);
        if (card2 is null)
        {
            return;
        }
        CardCmd.Upgrade(card2);
        await CardPileCmd.AddGeneratedCardToCombat(card2, PileType.Hand, Owner);
        CardModel card3 = Owner.Creature.CombatState.CreateCard<RollAgain>(Owner);
        if (card3 is null)
        {
            return;
        }
        CardCmd.Upgrade(card3);
        await CardPileCmd.AddGeneratedCardToCombat(card3, PileType.Hand, Owner);
        return;
    }

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<FakeStrikeDummy>();
    }
}