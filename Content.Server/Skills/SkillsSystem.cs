using Robust.Shared.Prototypes;
using Content.Shared.Skills;
using Robust.Server.GameObjects;

using System.Diagnostics.CodeAnalysis;

namespace Content.Server.Skills
{
    public sealed class SkillsSystem : EntitySystem
    {
        [Dependency] private readonly IPrototypeManager _prototypes = default!;

        [Dependency] private readonly UserInterfaceSystem _interfaceSystem = default!;

        private Dictionary<string, PerkDataPrototype> _perkToPrototype = new();

        private Dictionary<string, SkillPrototype> _skillToPrototype = new();

        private List<string> _publicSkills = new();

        private ISawmill _sawmill = default!;

        public int test = 0;

        private float _timeCounter = 0f;
        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<SkillsComponent, ComponentInit>(OnComponentInit);


            _sawmill = Logger.GetSawmill("Skills");
            _sawmill.Level = LogLevel.Info;

            LoadPrototypes();
            _prototypes.PrototypesReloaded += HandlePrototypesReloaded;

            SubscribeLocalEvent<SkillsComponent, SkillSyncRequestMessage>(OnSyncRequest);


        }
        private void HandlePrototypesReloaded(PrototypesReloadedEventArgs obj)
        {
            LoadPrototypes();
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
        public bool RetrievePerkPrototype(string identifier, [NotNullWhen(true)] out PerkDataPrototype? prototype)
        {
            if (_perkToPrototype.TryGetValue(identifier, out PerkDataPrototype? doWeHaveIt))
            {
                prototype = doWeHaveIt;
                return true;
            }
            prototype = null;
            return false;
        }

        public bool RetrieveSkillPrototype(string identifier, [NotNullWhen(true)] out SkillPrototype? skill)
        {
            if (_skillToPrototype.TryGetValue(identifier, out SkillPrototype? skillOrNot))
            {
                skill = skillOrNot;
                return true;
            }
            skill = null;
            return false;
        }

        protected private void LoadPrototypes()
        {
            _perkToPrototype.Clear();
            foreach (var perk in _prototypes.EnumeratePrototypes<PerkDataPrototype>())
            {
                if (_perkToPrototype.ContainsKey(perk.Name))
                {
                    Logger.ErrorS("skills",
                        "Found perk with duplicate PerkDataPrototype Name {0} - all perks must have" +
                        " a unique prototype, this one will be skipped", perk.Name);
                }
                else
                {
                    _sawmill.Log(LogLevel.Info, "Added perk prototype with {0} name", perk.Name);
                    _perkToPrototype.Add(perk.Name, perk);
                }
            }
            _skillToPrototype.Clear();
            _publicSkills.Clear();
            foreach (var skill in _prototypes.EnumeratePrototypes<SkillPrototype>())
            {
                if (_skillToPrototype.ContainsKey(skill.Name))
                {
                    Logger.ErrorS("skills",
                       "Found skill with duplicate SkillDataPrototype Name {0} - all skills must have" +
                       " a unique prototype, this one will be skipped", skill.Name);
                }
                else
                {
                    _sawmill.Log(LogLevel.Info, "Added skill prototype with {0} name", skill.Name);
                    _skillToPrototype.Add(skill.Name, skill);
                    if (skill.PublicSkill)
                        _publicSkills.Add(skill.Name);
                }
            }
        }

        public void OpenUI(EntityUid user, SkillsComponent component)
        {
            if (!TryComp<ActorComponent>(user, out var actor))
                return;
            _interfaceSystem.TryOpen(user, SkillMenuUiKey.Key, actor.PlayerSession);
        }

        private void OnSyncRequest(EntityUid uid, SkillsComponent component, SkillSyncRequestMessage args)
        {

            var skillCopy = new List<SkillHolder>(component.UserSkills);
            var perkCopy = new List<PerkHolder>(component.PerkIdentifiers);

            component.UserInterface?.SetState(new SkillSyncDataState(perkCopy, skillCopy));
        }

        public override void Update(float frameTime)
        {
            _timeCounter += frameTime;
            if(_timeCounter > 5)
            {
                foreach (SkillsComponent comp in EntityManager.EntityQuery<SkillsComponent>())
                {
                    _sawmill.Log(LogLevel.Info, "Tried to open UI for {0}", comp.Owner);
                    OpenUI(comp.Owner, comp);
                }
                _timeCounter -= 5;
            }
        }
    }
}
