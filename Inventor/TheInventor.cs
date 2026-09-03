using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using DiceTheSpire.Common.Extensions;
using DiceTheSpire.Common.Relics;
using DiceTheSpire.Inventor.Basic;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;

namespace DiceTheSpire.Inventor;

public class TheInventor : PlaceholderCharacterModel, ICustomEndTurnCharacter
{
    public const string CharacterId = "TheInventor";

    public static readonly Color Color = new("FFB458");

    public override Color NameColor => Color;

    public override Color EnergyLabelOutlineColor => Color;
    public override Color MapDrawingColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 62;
    public override IEnumerable<CardModel> StartingDeck => [
        StrikeInventor.Create(),
        StrikeInventor.Create(),
        StrikeInventor.Create(),
        DefendInventor.Create(),
        DefendInventor.Create(),
        DefendInventor.Create(),
        Spanner.Create(),
        Capacitor.Create()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics => [ModelDb.Relic<Manual>()];

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

    public override NCreatureVisuals CreateCustomVisuals()
    {
        return NodeFactory<NCreatureVisuals>.CreateFromResource(Path.Join(DiceTheSpire.MainFile.ResPath, "images", "inventor.png"));
    }


    public override string CustomMerchantAnimPath => Path.Join(DiceTheSpire.MainFile.ResPath, "merchant.tscn");
    public override string CustomIconTexturePath => "character_icon_the_inventor.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "charselect_inventor.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "charselect_unknown.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_the_inventor.png".CharacterUiPath();
    public ICustomEndTurnPingMachine Create(Player player) => new SimpleEndTurnPingMachine(false);
}