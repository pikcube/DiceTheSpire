using BaseLib.Utils;
using DiceTheSpireCore.DiceTheSpireCoreCode.Listeners;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace DiceTheSpireCore.DiceTheSpireCoreCode.Cards;

[Pool(typeof(StatusCardPool))]
public class Pip : DiceTheSpireCoreCard
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
        UpdatePips();
    }

    protected override void AfterCloned()
    {
        base.AfterCloned();
        ExecutionFinished -= Pip_ExecutionFinished;
        ExecutionFinished += Pip_ExecutionFinished;
        UpdatePips();
    }

    private void Pip_ExecutionFinished(AbstractModel obj)
    {
        UpdatePips();
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("PipDescription", "")];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Sly, CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => MutableHoverTips;
    private IEnumerable<IHoverTip> MutableHoverTips { get; set; } = [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DiceyHooks.OnModifyPipOnPlayAsync(choiceContext, cardPlay);
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

    private void UpdatePips()
    {
        if (RunState is null)
        {
            return;
        }

        IEnumerable<string> descriptions = RunState.IterateHookListeners(CombatState).OfType<IModifyPipOnPlayListener>()
            .Select(listener => listener.PipDescription.GetFormattedText());
        StringVar pipDescription = (StringVar)DynamicVars["PipDescription"];
        pipDescription.StringValue = string.Join("\n", descriptions);
        MutableHoverTips = RunState.IterateHookListeners(CombatState).OfType<IModifyPipOnPlayListener>()
            .SelectMany(listener => listener.PipHoverTips);
    }
}