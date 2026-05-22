using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheInventor.TheInventorCode.Gadgets;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class Chainsaw() : TheInventorCard(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{ 
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(26, DamageProps.card)];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<MinionPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        foreach (Creature c in CombatState.Enemies)
        {
            if (c.HasPower<MinionPower>())
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue * 2)
                    .FromCard(this)
                    .Targeting(c)
                    .WithHitFx(VfxCmd.slashPath)
                    .Execute(choiceContext);
            }
            else
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this)
                    .Targeting(c)
                    .WithHitFx(VfxCmd.slashPath)
                    .Execute(choiceContext);
            }
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string GetScrapId => nameof(DialUpSounds);
}