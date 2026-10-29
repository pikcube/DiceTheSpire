using Godot;

namespace DiceTheSpire.Shared.Extensions;

//Mostly utilities to get asset paths.
public static class StringExtensions
{
    extension(string path)
    {
        public string ImagePath()
        {
            return Path.Join(MainFile.ResPath, "images", path);
        }

        public string CardImagePath()
        {
            path = Path.Join(MainFile.ResPath, "images", "card_portraits", path);
            return ResourceLoader.Exists(path) ? path : Path.Join(MainFile.ResPath, "images", "card_portraits", "card.png");
        }

        public string BigCardImagePath()
        {
            path = Path.Join(MainFile.ResPath, "images", "card_portraits", "big", path);
            return ResourceLoader.Exists(path) ? path : Path.Join(MainFile.ResPath, "images", "card_portraits", "big", "card.png");
        }

        public string PowerImagePath()
        {
            path = Path.Join(MainFile.ResPath, "images", "powers", path);
            return ResourceLoader.Exists(path) ? path : Path.Join(MainFile.ResPath, "images", "powers", "power.png");
        }

        public string BigPowerImagePath()
        {
            path = Path.Join(MainFile.ResPath, "images", "powers", "big", path);
            return ResourceLoader.Exists(path) ? path : Path.Join(MainFile.ResPath, "images", "powers", "big", "power.png");
        }

        public string RelicImagePath()
        {
            path = Path.Join(MainFile.ResPath, "images", "relics", path);
            return ResourceLoader.Exists(path) ? path : Path.Join(MainFile.ResPath, "images", "relics", "relic.png");
        }

        public string BigRelicImagePath()
        {
            path = Path.Join(MainFile.ResPath, "images", "relics", "big", path);
            return ResourceLoader.Exists(path) ? path : Path.Join(MainFile.ResPath, "images", "relics", "big", "relic.png");
        }

        public string CharacterUiPath()
        {
            return Path.Join(MainFile.ResPath, "images", "charui", path);
        }

        public string PotionImagePath()
        {
            path = Path.Join(MainFile.ResPath, "images", "potions", path);
            return ResourceLoader.Exists(path) ? path : Path.Join(MainFile.ResPath, "images", "potions", "potion.png");
        }
    }
}