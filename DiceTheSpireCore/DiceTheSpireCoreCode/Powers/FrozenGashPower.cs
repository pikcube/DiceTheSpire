using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using global::DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using global::DiceTheSpireCore.DiceTheSpireCoreCode.Utilities;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Powers;


    public class FrozenGashPower : DiceTheSpireCorePower
    {

        public override PowerType Type => PowerType.Debuff;
        public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier;
        public override PowerStackType StackType => PowerStackType.Single;


        public override async Task AfterSideTurnEnd(
          PlayerChoiceContext choiceContext,
          CombatSide side,
          IEnumerable<Creature> participants)
        {
            if (side != CombatSide.Enemy || Owner is null || Applier is null || Applier.Player is null)
            {
                return;
            }
        await CreatureCmd.Damage(choiceContext, Owner, Applier.Player.Creature.Block, DamageProps.nonCardHpLoss, null, null);
    }

}

