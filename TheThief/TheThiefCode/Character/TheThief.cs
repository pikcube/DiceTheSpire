using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using TheThief.TheThiefCode.Cards.Basic;
using TheThief.TheThiefCode.Extensions;
using TheThief.TheThiefCode.Relics;

namespace TheThief.TheThiefCode.Character
{
    public class TheThief : PlaceholderCharacterModel
    {
        public const string CharacterId = "TheThief";

        public static readonly Color Color = new("1D7328");

        public override Color NameColor => Color;
        public override Color EnergyLabelOutlineColor => Color;
        public override Color MapDrawingColor => Color;

        public override CharacterGender Gender => CharacterGender.Masculine;
        public override int StartingHp => 70;

        public override IEnumerable<CardModel> StartingDeck => [
            ModelDb.Card<StrikeThief>(),
            ModelDb.Card<StrikeThief>(),
            ModelDb.Card<StrikeThief>(),
            ModelDb.Card<StrikeThief>(),
            ModelDb.Card<DefendThief>(),
            ModelDb.Card<DefendThief>(),
            ModelDb.Card<DefendThief>(),
            ModelDb.Card<DefendThief>(),
            ModelDb.Card<Peashooter>(),
            ModelDb.Card<Lockpick>()
        ];

        public override IReadOnlyList<RelicModel> StartingRelics =>
        [
            ModelDb.Relic<StickyFingers>()
        ];

        public override CardPoolModel CardPool => ModelDb.CardPool<TheThiefCardPool>();
        public override RelicPoolModel RelicPool => ModelDb.RelicPool<TheThiefRelicPool>();
        public override PotionPoolModel PotionPool => ModelDb.PotionPool<TheThiefPotionPool>();

        /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
            override all the other methods that define those assets. 
            These are just some of the simplest assets, given some placeholders to differentiate your character with. 
            You don't have to, but you're suggested to rename these images. */
        public override Control CustomIcon
        {
            get
            {
                var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
                icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
                return icon;
            }
        }
        public override string CustomIconTexturePath => "character_icon_the_thief.png".CharacterUiPath();
        public override string CustomCharacterSelectIconPath => "char_select_thief.png".CharacterUiPath();
        public override string CustomCharacterSelectLockedIconPath => "char_select_unknown.png".CharacterUiPath();
        public override string CustomMapMarkerPath => "map_marker_the_thief.png".CharacterUiPath();
    }
}