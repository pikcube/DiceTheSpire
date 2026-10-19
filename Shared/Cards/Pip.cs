using BaseLib.Utils;
using DiceTheSpire.Shared.Utility;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace DiceTheSpire.Shared.Cards;

[Pool(typeof(StatusCardPool))]
public class Pip : DiceTheSpireCard
{
    public Pip() : base(1, CardType.Status, CardRarity.Status, TargetType.None)
    {
        ExecutionFinished -= Pip_ExecutionFinished;
        ExecutionFinished += Pip_ExecutionFinished;
    }

    public override void AfterCreated()
    {
        base.AfterCreated();
        ExecutionFinished -= Pip_ExecutionFinished;
        ExecutionFinished += Pip_ExecutionFinished;
        UpdatePipText();
    }

    protected override void AfterCloned()
    {
        base.AfterCloned();
        ExecutionFinished -= Pip_ExecutionFinished;
        ExecutionFinished += Pip_ExecutionFinished;
        UpdatePipText();
    }

    private void Pip_ExecutionFinished(AbstractModel _)
    {
        UpdatePipText();
    }

    public override int MaxUpgradeLevel => 0;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(nameof(PipDescription))];
    public string PipDescription
    {
        get => ((StringVar)DynamicVars[nameof(PipDescription)]).StringValue;
        set
        {
            AssertMutable();
            ((StringVar)DynamicVars[nameof(PipDescription)]).StringValue = value;
        }
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Sly, CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => MutableHoverTips;
    private IEnumerable<IHoverTip> MutableHoverTips { get; set; } = [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DiceyHooks.OnModifyPipOnPlayAsync(cardPlay.Player.RunState, cardPlay.Player.Creature.CombatState, choiceContext, cardPlay, this);
    }

    public static async Task<IEnumerable<CardModel>> CreateInHandAsync(
        Player owner,
        int count,
        ICombatState combatState)
    {
        if (count == 0 || CombatManager.Instance.IsOverOrEnding)
        {
            return [];
        }

        List<CardModel> pips = [];

        for (int index = 0; index < count; ++index)
        {
            pips.Add(combatState.CreateCard<Pip>(owner));
        }

        await CardPileCmd.AddGeneratedCardsToCombat(pips, PileType.Hand, owner);

        return pips;
    }

    private void UpdatePipText()
    {
        if (CanonicalInstance == this || RunState is null)
        {
            return;
        }

        PipDescription = string.Join("\n", DiceyHooks.ModifyPipDescription(RunState, CombatState, this));
        
        MutableHoverTips = DiceyHooks.ModifyPipHoverTips(RunState, CombatState, this);
    }
}