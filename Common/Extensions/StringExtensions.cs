using Godot;

namespace DiceTheSpire.Common.Extensions;

//Mostly utilities to get asset paths.
public static class StringExtensions
{ 
    extension(string path)
    {
        public string ImagePath()
        {
            return Path.Join(DiceTheSpire.MainFile.ResPath, "images", path);
        }

        public string CardImagePath()
        {
            path = Path.Join(DiceTheSpire.MainFile.ResPath, "images", "card_portraits", path);
            if (ResourceLoader.Exists(path))
            {
                return path;
            }

            DiceTheSpire.MainFile.Logger.Info("Could not find card image path: " + path);
            return Path.Join(DiceTheSpire.MainFile.ResPath, "images", "card_portraits", "card.png");
        }

        public string BigCardImagePath()
        {
            path = Path.Join(DiceTheSpire.MainFile.ResPath, "images", "card_portraits", "big", path);
            if (ResourceLoader.Exists(path))
            {
                return path;
            }

            DiceTheSpire.MainFile.Logger.Info("Could not find big card image path: " + path);
            return Path.Join(DiceTheSpire.MainFile.ResPath, "images", "card_portraits", "big", "card.png");
        }

        public string PowerImagePath()
        {
            path = Path.Join(DiceTheSpire.MainFile.ResPath, "images", "powers", path);
            if (ResourceLoader.Exists(path))
            {
                return path;
            }

            DiceTheSpire.MainFile.Logger.Info("Could not find power image path: " + path);
            return Path.Join(DiceTheSpire.MainFile.ResPath, "images", "powers", "power.png");
        }

        public string BigPowerImagePath()
        {
            path = Path.Join(DiceTheSpire.MainFile.ResPath, "images", "powers", "big", path);
            if (ResourceLoader.Exists(path))
            {
                return path;
            }

            DiceTheSpire.MainFile.Logger.Info("Could not find big power image path: " + path);
            return Path.Join(DiceTheSpire.MainFile.ResPath, "images", "powers", "big", "power.png");
        }

        public string RelicImagePath()
        {
            path = Path.Join(DiceTheSpire.MainFile.ResPath, "images", "relics", path);
            if (ResourceLoader.Exists(path))
            {
                return path;
            }

            DiceTheSpire.MainFile.Logger.Info("Could not find relic image path: " + path);
            return Path.Join(DiceTheSpire.MainFile.ResPath, "images", "relics", "relic.png");
        }

        public string BigRelicImagePath()
        {
            path = Path.Join(DiceTheSpire.MainFile.ResPath, "images", "relics", "big", path);
            if (ResourceLoader.Exists(path))
            {
                return path;
            }

            DiceTheSpire.MainFile.Logger.Info("Could not find big relic image path: " + path);
            return Path.Join(DiceTheSpire.MainFile.ResPath, "images", "relics", "big", "relic.png");
        }

        public string CharacterUiPath()
        {
            return Path.Join(DiceTheSpire.MainFile.ResPath, "images", "charui", path);
        }

        public string PotionImagePath()
        {
            path = Path.Join(DiceTheSpire.MainFile.ResPath, "images", "potions", path);
            if (ResourceLoader.Exists(path))
            {
                return path;
            }

            DiceTheSpire.MainFile.Logger.Info("Could not find potion image path: " + path);
            return Path.Join(DiceTheSpire.MainFile.ResPath, "images", "potions", "potion.png");
        }
    }
}