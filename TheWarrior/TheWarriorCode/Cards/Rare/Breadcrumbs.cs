using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWarrior.TheWarriorCode.Cards.Rare
{

    public class Breadcrumbs() : TheWarriorCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DexterityPower>(1M), new BlockVar(1, BlockProps.card)];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, DynamicVars.Dexterity.BaseValue, Owner.Creature, this);
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            //await CardPileCmd.Add(this, PileType.Draw, CardPilePosition.Random);
        }

        public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay,
            ResourceInfo resources,
            CardLocation cardLocation)
        {
            return card == this && cardLocation.pileType == PileType.Discard ? new CardLocation(cardLocation.player, PileType.Draw, CardPilePosition.Random) : cardLocation;
        }
        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Sly);
        }
    }
}