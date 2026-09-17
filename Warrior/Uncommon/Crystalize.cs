using BaseLib.Extensions;
using DiceTheSpire.Shared.Interfaces;
using DiceTheSpire.Shared.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DiceTheSpire.Warrior.Uncommon;

public class Crystalize() : TheWarriorCard(-1, CardType.Power, CardRarity.Rare, TargetType.Self), ICrystalCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];
    protected override bool HasEnergyCostX => true;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is null)
        {
            return;
        }

        int xValue = cardPlay.Card.ResolveEnergyXValue();

        if (IsUpgraded)
        {
            await PowerCmd.Apply<CrystalizePlusPower>(choiceContext, Owner.Creature, xValue, Owner.Creature, this);
        }
        else
        { 
            await PowerCmd.Apply<CrystalizePower>(choiceContext, Owner.Creature, xValue, Owner.Creature, this);
        }

    }

    protected override void OnUpgrade()
    {
        base.OnUpgrade();
    }
}

