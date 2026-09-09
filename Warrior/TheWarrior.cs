using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using DiceTheSpire.Shared.Extensions;
using DiceTheSpire.Shared.Relics;
using DiceTheSpire.Warrior.Basic;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace DiceTheSpire.Warrior;

public class TheWarrior : PlaceholderCharacterModel
{
    public const string CharacterId = "TheWarrior";

    public static readonly Color Color = new("7BC8FF");

    public override Color NameColor => Color;
    public override Color EnergyLabelOutlineColor => Color;
    public override Color MapDrawingColor => Color;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 80;

    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeWarrior>(),
        ModelDb.Card<StrikeWarrior>(),
        ModelDb.Card<StrikeWarrior>(),
        ModelDb.Card<StrikeWarrior>(),
        ModelDb.Card<Sword>(),
        ModelDb.Card<DefendWarrior>(),
        ModelDb.Card<DefendWarrior>(),
        ModelDb.Card<DefendWarrior>(),
        ModelDb.Card<DefendWarrior>(),
        ModelDb.Card<DefendWarrior>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<CombatRoll>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<TheWarriorCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<TheWarriorRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<TheWarriorPotionPool>();

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

    public override NCreatureVisuals CreateCustomVisuals()
    {
        return NodeFactory<NCreatureVisuals>.CreateFromResource(Path.Join(MainFile.ResPath, "images", "warrior.png"));
    }


    public override string CustomIconTexturePath => "character_icon_the_warrior.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_warrior.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_unknown.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_the_warrior.png".CharacterUiPath();
}