using BaseLib.Extensions;
using DiceTheSpire.Inventor.Gadgets;
using DiceTheSpire.Shared.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace DiceTheSpire.Inventor.Rare;


public class RosewoodSpear() : TheInventorCard(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, DamageProps.card)];

    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<ThornsPower>()];
    public override string GetScrapId => nameof(Needle);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        if (CombatState is null)
        {
            return;
        }

        ThornsPower? thorns = Owner.Creature.GetPower<ThornsPower>();
        if (thorns is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState)
            .WithValueProp(DynamicVars.Damage.Props)
            .WithHitFx(VfxCmd.slashPath)
            .WithHitCount(thorns.Amount)
            .Execute(choiceContext);
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card == this && cardPlay.IsLastInSeries)
        {
            await PowerCmd.Remove<ThornsPower>(Owner.Creature);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}