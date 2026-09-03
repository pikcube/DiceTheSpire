using DiceTheSpire.Common.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Thief.Rare;

public class SignalJammer() : TheThiefCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(9, ValueProp.Move)];
    public override bool GainsBlock => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Pip>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null || Owner.PlayerCombatState is null)
        {
            return;
        }

        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        int cardsToAdd = CardPile.MaxCardsInHand - Owner.PlayerCombatState.Hand.Cards.Count;
        await Pip.CreateInHandAsync(Owner, cardsToAdd, CombatState);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
}