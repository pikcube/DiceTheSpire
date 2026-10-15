//using BaseLib.Extensions;
//using DiceTheSpire.Shared.Powers;
//using MegaCrit.Sts2.Core.CardSelection;
//using MegaCrit.Sts2.Core.Commands;
//using MegaCrit.Sts2.Core.Context;
//using MegaCrit.Sts2.Core.Entities.Cards;
//using MegaCrit.Sts2.Core.GameActions.Multiplayer;
//using MegaCrit.Sts2.Core.HoverTips;
//using MegaCrit.Sts2.Core.Localization.DynamicVars;
//using MegaCrit.Sts2.Core.Models;
//using MegaCrit.Sts2.Core.Models.Cards;
//using MegaCrit.Sts2.Core.Models.Powers;
//using MegaCrit.Sts2.Core.Nodes;
//using MegaCrit.Sts2.Core.Nodes.Vfx;
//using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
//using MegaCrit.Sts2.Core.TestSupport;
//using Pikcube.Common.Powers;

//namespace DiceTheSpire.Warrior.Rare;

//public class NightmareCrystal() : TheWarriorCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
//{


//    protected override IEnumerable<string> ExtraRunAssetPaths => NNightmareHandsVfx.AssetPaths;

//    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//    {
//        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
//        IEnumerable<CardModel> cards = await CardSelectCmd.FromHand(choiceContext, Owner, prefs, (Func<CardModel, bool>)null, (AbstractModel));
//        //if (LocalContext.IsMe(Owner))
//        //{
//        //    NGame.Instance.CurrentRunNode.GlobalUi.AddChildSafely((Godot.Node)NSmokyVignetteVfx.Create(new Godot.Color(0.8f, 0.3f, 0.8f, 0.66f), new Godot.Color(0.0f, 0.0f, 4f, 0.33f)));
//        //    NGame.Instance.CurrentRunNode.GlobalUi.AddChildSafely((Godot.Node)NNightmareHandsVfx.Create());
//        //    await Cmd.CustomScaledWait(0.1f, 0.25f);
//        //}
//        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
//        CardModel selectedCard = cards.FirstOrDefault<CardModel>();
//        if (selectedCard == null)
//        {
//            cards = (IEnumerable<CardModel>)null;
//            selectedCard = (CardModel)null;
//        }
//        else
//        {
//            (await PowerCmd.Apply<NightmarePower>(choiceContext, Owner.Creature, 3M, Owner.Creature, (CardModel)this)).SetSelectedCard(selectedCard);
//            cards = (IEnumerable<CardModel>)null;
//            selectedCard = (CardModel)null;
//        }
//    }

//    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
//}


