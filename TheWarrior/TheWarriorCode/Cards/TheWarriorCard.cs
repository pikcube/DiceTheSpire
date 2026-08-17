using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using DiceTheSpireCore.DiceTheSpireCoreCode.Interfaces;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheWarrior.TheWarriorCode.Character;
using TheWarrior.TheWarriorCode.Extensions;

namespace TheWarrior.TheWarriorCode.Cards;

[Pool(typeof(TheWarriorCardPool))]
public abstract class TheWarriorCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target), IPipCard
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public Texture2D GetPips(int? cost, bool isPretend, CardCostColor? energyCostColor = null)
    {
        return PipCard.GetPipsForMod(this, MainFile.ResPath, cost, isPretend, energyCostColor);
    }

    public CardLocation PublicGetResultLocationForCardPlay() => GetResultLocationForCardPlay();

    public Task<int> PublicGeneratePlayCount(ICombatState combatState, Creature? target) => GeneratePlayCount(combatState, target);

    public Task PublicOnPlay(BranchingPlayerChoiceContext branchingPlayerChoiceContext, CardPlay cardPlay) => OnPlay(branchingPlayerChoiceContext, cardPlay);
}