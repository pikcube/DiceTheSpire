using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Utility;
using TheInventor.TheInventorCode.Cards;
using TheInventor.TheInventorCode.Extensions;
using TheInventor.TheInventorCode.Relics;

namespace TheInventor.TheInventorCode.Character
{
    public class TheInventor : PlaceholderCharacterModel
    {
        public const string CharacterId = "TheInventor";
        public static bool HideGadget { get; set; }

        public static readonly Color Color = new("ffffff");

        public override Color NameColor => Color;
        public override CharacterGender Gender => CharacterGender.Feminine;
        public override int StartingHp => 70;

        public override IEnumerable<CardModel> StartingDeck => [
            ModelDb.Card<StrikeInventor>(),
            ModelDb.Card<StrikeInventor>(),
            ModelDb.Card<StrikeInventor>(),
            ModelDb.Card<DefendInventor>(),
            ModelDb.Card<DefendInventor>(),
            ModelDb.Card<DefendInventor>()
        ];

        public override IReadOnlyList<RelicModel> StartingRelics => [ModelDb.Relic<Manual>(), ModelDb.Relic<Gadget>()];

        public override CardPoolModel CardPool => ModelDb.CardPool<TheInventorCardPool>();
        public override RelicPoolModel RelicPool => ModelDb.RelicPool<TheInventorRelicPool>();
        public override PotionPoolModel PotionPool => ModelDb.PotionPool<TheInventorPotionPool>();

        /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
            override all the other methods that define those assets. 
            These are just some of the simplest assets, given some placeholders to differentiate your character with. 
            You don't have to, but you're suggested to rename these images. */
        public override Control CustomIcon
        {
            get
            {
                Control icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
                icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
                return icon;
            }
        }
        public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
        public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
        public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
        public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
    }
}