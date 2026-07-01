using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.OpenXRInterface;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers
{
    public class BicepCurlPower : DiceTheSpireCorePower, IAfterNudgeListener
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public async Task AfterNudgeAsync(PlayerChoiceContext choiceContext, CardModel card, bool wasExhausted)
        {
            if (Owner.Player?.PlayerCombatState is null)
            {
                return;
            }
            PlayerCombatState pcs = Owner.Player.PlayerCombatState;
            CardModel[] allCards = [.. pcs.Hand.Cards, .. pcs.DrawPile.Cards, .. pcs.DiscardPile.Cards];

            CardCmd.Upgrade(allCards.TakeRandom(Amount, Owner.Player.RunState.Rng.CombatCardSelection), CardPreviewStyle.HorizontalLayout);
        }
    }
}
