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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace TheWarrior.TheWarriorCode.Relics;
[UsedImplicitly]
public class CombatRoll : TheWarriorRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(BetterStaticHoverTips.Reroll), HoverTipFactory.FromCard<RollAgain>(true)];
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (Owner != player || Owner.Creature.CombatState is null || Owner.PlayerCombatState?.TurnNumber != 1)
        {
            return;
        }

        for (int n = 0; n < 3; ++n)
        {
            RollAgain card = combatState.CreateCard<RollAgain>(Owner);
            CardCmd.Upgrade(card);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
    }

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<FakeStrikeDummy>();
    }
}