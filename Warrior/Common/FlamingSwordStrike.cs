using BaseLib.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using DiceTheSpire.Shared.Utility;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Warrior.Common;
public class FlamingSwordStrike() : TheWarriorCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy), IFuryModifier
{
    private int FlaimingSwordsBeingPlayed
    {
        get;
        set => field = value < 0 ? 0 : value;
    } = 0;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10M, DamageProps.card), new PowerVar<FuryPower>(1M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FuryPower>(), HoverTipFactory.FromPower<VigorPower>()];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount,
        Creature? applier,
        out decimal modifiedAmount)
    {
        if (canonicalPower is not VigorPower vigorPower || target != Owner.Creature || amount >= 0 || FlaimingSwordsBeingPlayed == 0)
        {
            modifiedAmount = amount;
            return false;
        }

        vigorPower.PrivateFieldWrapper<PowerModel, object>("_internalData").Value = 
            AccessTools.DeclaredMethod(typeof(VigorPower), "InitInternalData").Invoke(vigorPower, []);

        modifiedAmount = 0;
        return true;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        ++FlaimingSwordsBeingPlayed;
        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
            .FromCard(this, cardPlay)
            .WithHitFx(VfxCmd.slashPath)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        --FlaimingSwordsBeingPlayed;

    }


    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public bool ShouldIgnoreFury => false;
    public bool ShouldMaintainFury => true;
}