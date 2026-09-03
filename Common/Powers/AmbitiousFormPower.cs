using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DiceTheSpire.Common.Powers;

public class AmbitiousFormPower : TheThiefPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    protected override object InitInternalData() => new Data();
    public override int DisplayAmount => Amount - GetInternalData<Data>().CardsPlayed % Amount;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner || cardPlay.Resources.EnergySpent != 1)
        {
            return;
        }

        Data data = GetInternalData<Data>();
        data.CardsPlayed += 1;
        if (data.CardsPlayed >= Amount)
        { 
            Flash();
            await PlayerCmd.GainEnergy(1, cardPlay.Card.Owner);
            data.CardsPlayed -= Amount;
        }
        InvokeDisplayAmountChanged();

    }

    private class Data
    {
        public int CardsPlayed;
    }
}