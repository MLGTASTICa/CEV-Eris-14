using Robust.Shared.Serialization;
using JetBrains.Annotations;
using System.Collections.Generic;
using Robust.Shared.Utility;
using Robust.Shared.Network;
using Robust.Shared.GameStates;
using System.Globalization;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Skills
{
    // Just as a notice , a byte can only have 8 flags, so if further skills get added , it should get changed to a short 
    [Flags]
    public enum Skills : byte
    {
        Robustness = 0,
        Toughness = 1<<0,
        Vigilance = 1<<1,
        Mechanical = 1<<2,
        Cognition = 1<<3,
        Biology = 1<<4,
    }

    [Flags]
    public enum Perks : short
    {
        BallsOfPlasteel = 0,

    }
    [Prototype("perk")]
    public sealed class PerkDataPrototype : IPrototype
    {
        string IPrototype.ID => AssociatedPerk.ToString();
        [DataField("perk")]
        public Perks AssociatedPerk = Perks.BallsOfPlasteel;

        [DataField("description")]
        public string Description = "You are far more tougher to pain compared to other spacemen.";

        [DataField("icon")]
        public SpriteSpecifier Icon = new SpriteSpecifier.Texture(new ResourcePath("Textures/noSprite.png"));
    }


    [NetworkedComponent()]
    public abstract class SharedSkillsComponent : Component
    {
        public List<Perks> AcquiredPerks = new List<Perks>();

        public Dictionary<Skills, int> SkillValues = new()
        {
            { Skills.Robustness, 0},
            { Skills.Toughness, 0 },
            { Skills.Vigilance, 0},
            { Skills.Mechanical, 0},
            { Skills.Cognition, 0},
            { Skills.Biology, 0},

        };
    }
}
