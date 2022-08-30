/*
using Robust.Shared.Prototypes;
using Robust.Shared.Log;
using Robust.Shared.GameStates;
using System.Diagnostics.CodeAnalysis;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Shared.Skills
{
    public abstract class SharedSkillsSystem : EntitySystem
    {
        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<ComponentInit, SharedSkillsComponent>(OnComponentInit);


        }
        private void OnComponentInit(EntityUid uid, SharedSkillsComponent component, ComponentInit args)
        {
            base.Initialize();
            component.UserSkills.Clear();
            component.PerkIdentifiers.Clear();
            foreach (string skillIdentifier in _publicSkills)
            {
                SkillHolder skillData = new(skillIdentifier, 0);
                component.UserSkills.Add(skillData);
            }


        }

    }
}
*/
