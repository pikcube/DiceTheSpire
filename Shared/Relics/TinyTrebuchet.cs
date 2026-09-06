using DiceTheSpire.Shared.Listeners;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Relics;

public class TinyTrebuchet : TheThiefRelic, IModifyPipOnPlayListener
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public LocString PipDescription => new LocString("relics", Id.Entry + ".pipDescription").WithDynamicVars(DynamicVars);

    public IEnumerable<IHoverTip> PipHoverTips => [];

    public async Task ModifyPipOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature.CombatState is null || cardPlay.Card.Owner != Owner)
        {
            return;
        }

        await DamageCmd.Attack(4).FromCard(cardPlay.Card, cardPlay).TargetingRandomOpponents(Owner.Creature.CombatState)
            .Execute(choiceContext);
    }
}