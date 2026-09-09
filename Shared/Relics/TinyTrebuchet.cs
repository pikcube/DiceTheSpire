using DiceTheSpire.Shared.Cards;
using DiceTheSpire.Shared.Listeners;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;

namespace DiceTheSpire.Shared.Relics;

public class TinyTrebuchet : TheThiefRelic, IModifyPipOnPlayListener
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move)];

    public LocString PipDescription
    {
        get
        {
            LocString l = new LocString("relics", Id.Entry + ".pipDescription").WithDynamicVars(DynamicVars);
            l.Add("Damage", DynamicVars.Damage.BaseValue);
            return l;
        }
    }

    public IEnumerable<IHoverTip> PipHoverTips => [];

    public async Task ModifyPipOnPlayAsync(PlayerChoiceContext choiceContext, CardPlay cardPlay, Pip pip)
    {
        if (Owner.Creature.CombatState is null || !ShouldModify(pip))
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(pip, cardPlay).TargetingRandomOpponents(Owner.Creature.CombatState)
            .Unpowered().Execute(choiceContext);
    }

    public bool ShouldModify(Pip pip) => pip.Owner == Owner;
}