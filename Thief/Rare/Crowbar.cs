using DiceTheSpire.Common.Extensions;
using DiceTheSpire.Common.Interfaces;
using DiceTheSpire.Common.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DiceTheSpire.Thief.Rare;

  public class Crowbar() : TheThiefCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
  {
      protected override bool HasEnergyCostX => true;
      protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CountdownModel.Countdown)];

      protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
      {
          if (CombatState is null)
          {
              return;
          }

          int xvalue = ResolveEnergyXValue();
          if (IsUpgraded)
          {
              xvalue += 1;
          }
          foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards)
          {
              if (card is ICountdown countdown)
              { 
                  await countdown.DecrementCountAsync(xvalue);
              }
          }
      }
  }