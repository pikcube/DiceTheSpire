using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheThief.TheThiefCode.Cards.Uncommon;

namespace TheThief.TheThiefCode.Cards.Common;

  
public class RosewoodSpear() : TheThiefCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6M, ValueProp.Move), new PowerVar<ThornsPower>(2)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ThornsPower>()];
    

    private Decimal _thornsGainedThisTurn;
    private Decimal ThornsGainedThisTurn
    {
        get => this._thornsGainedThisTurn;
        set
        {
            this.AssertMutable();
            this._thornsGainedThisTurn = value;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath).Execute(choiceContext);
        await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature, DynamicVars["ThornsPower"].BaseValue, Owner.Creature, this);
        ThornsGainedThisTurn += DynamicVars["ThornsPower"].BaseValue;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars["ThornsPower"].UpgradeValueBy(1);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (ThornsGainedThisTurn > 0 && Owner.HasPower<ThornsPower>())
        {
            decimal amtToModifyBy = ThornsGainedThisTurn * -1;
            await PowerCmd.ModifyAmount(choiceContext, Owner.Creature.GetPower<ThornsPower>(), amtToModifyBy, Owner.Creature, this);
        }

        ThornsGainedThisTurn = 0;
    }
}