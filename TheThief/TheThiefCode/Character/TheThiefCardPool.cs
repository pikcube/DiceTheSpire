using BaseLib.Abstracts;
using Godot;
using TheThief.TheThiefCode.Extensions;

namespace TheThief.TheThiefCode.Character;

public class TheThiefCardPool : CustomCardPoolModel
{
    public override string Title => TheThief.CharacterId; //This is not a display name.

    public override string BigEnergyIconPath => "charui/Energy/ui_dice_dice0.png".ImagePath();
    public override string TextEnergyIconPath => "charui/dice_energy.png".ImagePath();


    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 0.39f; //Hue; changes the color.
    public override float S => 0.84f; //Saturation
    public override float V => 0.75f; //Brightness

    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load TheThief/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/

    //Color of small card icons
    public override Color DeckEntryCardColor => new("1e6331");

    public override bool IsColorless => false;
}