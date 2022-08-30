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
using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Actions;
using Content.Shared.Actions.ActionTypes;
using Content.Shared.Damage;
using Content.Shared.Destructible;
using Content.Shared.Emag.Systems;
using Content.Shared.Throwing;
using Content.Shared.VendingMachines;
using Robust.Shared.Audio;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Shared.Actions.ActionTypes;
using Content.Shared.Sound;
using Content.Shared.VendingMachines;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Skills
{
    [Prototype("perk")]
    public sealed class PerkDataPrototype : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; } = default!;

        [DataField("name")]
        public string Name = "Debug perk";

        [DataField("description")]
        public string Description = "You can now commit numerous exploits!";

        [DataField("icon")]
        public SpriteSpecifier.Texture Icon = new SpriteSpecifier.Texture(new ResourcePath("Textures/noSprite.png"));
    }

    [Prototype("skill")]

    public sealed class SkillPrototype : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; } = default!;

        [DataField("name")]
        public string Name = "NoSkill?";

        [DataField("max")]
        public ushort MaxValue = 999;

        [DataField("min")]
        public short MinValue = -999;

        // does this skill show by default in the skills tab?
        [DataField("publicSkill")]
        public bool PublicSkill = false;

    }

    [NetSerializable, Serializable]

    public sealed class PerkHolder
    {
        public readonly string Name;
        public readonly string Description;
        public readonly SpriteSpecifier.Texture Icon;

        public PerkHolder(string name, string description, SpriteSpecifier.Texture icon)
        {
            Name = name;
            Description = description;
            Icon = icon;
        }
    }

    [NetSerializable, Serializable]
    public sealed class SkillHolder
    {
        [ViewVariables(VVAccess.ReadOnly)] public readonly string Name;
        [ViewVariables(VVAccess.ReadWrite)] public short Value;
        public SkillHolder(string name, short value)
        {
            Name = name;
            Value = value;
        }
    }

    [NetworkedComponent()]
    public abstract class SharedSkillsComponent : Component
    {
        [ViewVariables(VVAccess.ReadOnly)]
        public List<PerkHolder> PerkIdentifiers = new();

        [ViewVariables(VVAccess.ReadOnly)]
        public List<SkillHolder> UserSkills = new();

    }


    [Serializable, NetSerializable]
    public sealed class SkillSyncRequestMessage : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public enum SkillMenuUiKey
    {
        Key,
    }

    [Serializable, NetSerializable]
    public sealed class SkillSyncDataState : BoundUserInterfaceState
    {
        public readonly List<PerkHolder> Perks;

        public readonly List<SkillHolder> Skills;
        public SkillSyncDataState(List<PerkHolder> perks, List<SkillHolder> skills)
        {
            Perks = perks;
            Skills = skills;
        }
    }
}
