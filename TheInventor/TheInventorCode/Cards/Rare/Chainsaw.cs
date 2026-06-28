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
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraInventorHoverTips => [HoverTipFactory.FromPower<MinionPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        foreach (Creature c in CombatState.Enemies.ToArray())
        {
            if (c.HasPower<MinionPower>())
            {
                await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue * 2)
                    .FromCard(this)
                    .Targeting(c)
                    .WithHitFx(VfxCmd.slashPath)
                    .Execute(choiceContext);
            }
            else
            {
                await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
                    .FromCard(this)
                    .Targeting(c)
                    .WithHitFx(VfxCmd.slashPath)
                    .Execute(choiceContext);
            }
        }
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }

    public override string GetScrapId => nameof(DialUpSounds);
}