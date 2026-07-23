using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheThief.TheThiefCode.Enchantments;

public class Stellar : CustomEnchantmentModel
{
    public override bool HasExtraCardText => true;
    public override bool ShowAmount => false;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StarsVar(1)];

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        if (cardPlay is null)
        {
            return;
        }
        await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, cardPlay.Player);
    }
}