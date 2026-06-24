using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Pikcube.Common.Extensions;
using TheInventor.TheInventorCode.Gadgets;
using TheInventor.TheInventorCode.Powers;

namespace TheInventor.TheInventorCode.Cards.Rare;

public class StallOfMirrors() : TheInventorCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public static SavedSpireField<Player, int> CurrentStall = new(() => 0, $"{MainFile.ModId}_{nameof(CurrentStall)}");

    public override string GetScrapId => nameof(MagicDice);
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await StallOfMirrorsPower.ApplyAsync(choiceContext, Owner.Creature, DynamicVars.Energy.EnchantedValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}