using DiceTheSpire.DiceTheSpireCode.Common.Interfaces;
using DiceTheSpire.DiceTheSpireCode.Common.Powers;
using DiceTheSpire.DiceTheSpireCode.Common.Utility;
using DiceTheSpire.DiceTheSpireCode.Inventor.Gadgets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace DiceTheSpire.DiceTheSpireCode.Inventor.Rare;


public class EncyclopedicForm() : TheInventorCard(3, CardType.Power, CardRarity.Rare, TargetType.Self), IScrapCard
{ 
    public override string GetScrapId => nameof(BurstOfKnowledge);

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.Static(InventorStaticHoverTips.TemporaryGadget)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        TemporaryGadgetPower? power = await PowerCmd.Apply<TemporaryGadgetPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        if (power is null)
        {
            return;
        }

        await power.RandomizeThisAsync();
        await power.LinkedGadgetModel.OnRechargeAsync(choiceContext, Owner);
    }

    public bool IsAlwaysOfferedAsScrap => !IsUpgraded;
    public EncyclopedicForm Card => this;
}