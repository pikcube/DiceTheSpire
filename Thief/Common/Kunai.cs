using BaseLib.Cards.Variables;
using DiceTheSpire.Common.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Thief.Common;

public class Kunai() : TheThiefCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Pip>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        if (CombatState is null)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
            .Execute(choiceContext);
        await Pip.CreateInHandAsync(Owner, 1, CombatState);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
        DynamicVars.PrivateFieldWrapper<DynamicVarSet, Dictionary<string, DynamicVar>>("_vars").Value?[PersistVar.Key] =
            new PersistVar(2);
    }
}