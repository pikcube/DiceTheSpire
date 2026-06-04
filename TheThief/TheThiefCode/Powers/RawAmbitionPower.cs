using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheThief.TheThiefCode.Powers;

public class RawAmbitionPower : TheThiefPower
{
    private const int CardPlayCount = 3;
    private bool _triggerOnSourcePlay = true;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    protected override object InitInternalData() => new Data();
    public override int DisplayAmount => CardPlayCount - GetInternalData<Data>().CardsPlayed % CardPlayCount;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(CardPlayCount)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (_triggerOnSourcePlay)
        {
            _triggerOnSourcePlay = false;
            return;
        }
        if (cardPlay.Card.Owner.Creature != Owner || cardPlay.Resources.EnergySpent != 1)
        {
            return;
        }

        Data data = GetInternalData<Data>();
        data.CardsPlayed += 1;
        if (data.CardsPlayed >= CardPlayCount)
        { 
            Flash();
            await PlayerCmd.GainEnergy(1, cardPlay.Card.Owner);
            data.CardsPlayed -= CardPlayCount;
        }
        InvokeDisplayAmountChanged();

    }

    private class Data
    {
        public int CardsPlayed;
    }
}