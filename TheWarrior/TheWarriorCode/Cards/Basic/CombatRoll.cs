using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheWarrior.TheWarriorCode.Cards.Uncommon;

namespace TheWarrior.TheWarriorCode.Cards.Basic;


public class CombatRoll() : TheWarriorCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
{

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner));
        await Cmd.Wait(0.25f);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner));
        await Cmd.Wait(0.25f);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<RollAgain>(Owner), PileType.Hand, Owner));
        await Cmd.Wait(0.25f);

    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

}
