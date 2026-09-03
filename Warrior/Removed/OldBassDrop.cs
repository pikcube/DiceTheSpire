//using MegaCrit.Sts2.Core.Commands;
//using MegaCrit.Sts2.Core.Entities.Cards;
//using MegaCrit.Sts2.Core.GameActions.Multiplayer;
//using MegaCrit.Sts2.Core.HoverTips;
//using MegaCrit.Sts2.Core.Localization.DynamicVars;
//using MegaCrit.Sts2.Core.Models.Powers;

//namespace TheWarrior.TheWarriorCode.Cards.Removed;
//public class OldBassDrop() : TheWarriorCard(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
//{
//    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
//    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<VulnerablePower>()];
//    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VulnerablePower>(2), new PowerVar<WeakPower>(2)];

//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        if (CombatState is null)
//        {
//            return;
//        }

//        if (CombatState?.RoundNumber % 2 == 0 != IsUpgraded)
//        {
//            ArgumentNullException.ThrowIfNull(cardPlay.Target);
//            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.IntValue,Owner.Creature, cardPlay.Card);
//        }
//        else
//        {
//            //await PowerCmd.Apply<WeakPower>(choiceContext, Owner.Creature, DynamicVars.Power<WeakPower>().IntValue, Owner.Creature, this);
//            //await PowerCmd.Apply<WeakPower>(choiceContext, CombatState?.Enemies, DynamicVars.Vulnerable.IntValue, Owner.Creature, cardPlay.Card);
//            await PowerCmd.Apply<WeakPower>(choiceContext, CombatState?.Creatures, DynamicVars.Weak.IntValue, Owner.Creature, cardPlay.Card);
//        }

//    }

//    public override TargetType TargetType => CombatState?.RoundNumber % 2 == 0 != IsUpgraded ? TargetType.AnyEnemy : TargetType.Self;
//    protected override bool ShouldGlowGoldInternal => CombatState?.RoundNumber % 2 == 0 != IsUpgraded;
//    protected override void OnUpgrade()
//    {
//        base.OnUpgrade();
//        RemoveKeyword(CardKeyword.Ethereal);
//    }
//}

//"THEWARRIOR-BASS_DROP.description": "On {IfUpgraded:show:EVEN|ODD} turns,\napply {WeakPower:diff()} [gold]Weak[/gold] to EVERYONE.\nOn {IfUpgraded:show:ODD|EVEN} turns,\napply {VulnerablePower:diff()} [gold]Vulnerable[/gold] to an enemy.",
//"THEWARRIOR-BASS_DROP.title": "Bass Drop",
    

