//using MegaCrit.Sts2.Core.Commands;
//using MegaCrit.Sts2.Core.Entities.Cards;
//using MegaCrit.Sts2.Core.GameActions.Multiplayer;
//using MegaCrit.Sts2.Core.Localization.DynamicVars;
//using MegaCrit.Sts2.Core.ValueProps;

//namespace TheWarrior.TheWarriorCode.Cards.Common;

//public class BattleAxe() : TheWarriorCard(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
//{
//    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, DamageProps.card), new RepeatVar(1)];

//    public static event Action? OnHitCountChanged;
//    public static int HitCount
//    {
//        get;
//        set
//        {
//            field = value;
//            OnHitCountChanged?.Invoke();
//        }
//    } = 1;
//    public override Task BeforeCombatStart()
//    {
//        DynamicVars.Repeat.BaseValue = 1;
//        HitCount = 1;
//        return Task.CompletedTask;
//    }

//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        ArgumentNullException.ThrowIfNull(cardPlay.Target);

//        await DamageCmd.Attack(DynamicVars.Damage.EnchantedValue)
//            .WithHitCount(HitCount)
//            .FromCard(this, cardPlay)
//            .WithHitFx(VfxCmd.slashPath)
//            .Targeting(cardPlay.Target)
//            .Execute(choiceContext);

//        HitCount += 1;

//    }
//    protected override void AfterDowngraded()
//    {
//        DynamicVars.Repeat.BaseValue = HitCount;
//        OnHitCountChanged?.Invoke();
//    }
//    protected override void OnUpgrade()
//    {
//        EnergyCost.UpgradeBy(-1);
//        OnHitCountChanged?.Invoke();
//    }
//    protected override void AfterCloned()
//    {
//        base.AfterCloned();
//        OnHitCountChanged += () => DynamicVars.Repeat.BaseValue = 1;
//    }
//}