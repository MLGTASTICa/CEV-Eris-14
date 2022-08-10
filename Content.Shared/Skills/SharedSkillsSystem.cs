using Robust.Shared.Prototypes;

namespace Content.Shared.Skills
{
    public abstract class SharedSkillsSystem : EntitySystem
    {
        [Dependency] private readonly PrototypeManager _prototypes = default!;

        private readonly Dictionary<Perks, PerkDataPrototype> _perkToPrototype = new();

        public override void Initialize()
        {
            base.Initialize();

            LoadPrototypes();
            _prototypes.PrototypesReloaded += HandlePrototypesReloaded;
        }

        private void HandlePrototypesReloaded(PrototypesReloadedEventArgs obj)
        {
            LoadPrototypes();
        }

        protected private void LoadPrototypes()
        {
            _perkToPrototype.Clear();
            foreach (var perk in _prototypes.EnumeratePrototypes<PerkDataPrototype>())
            {
                if (!_perkToPrototype.TryAdd(perk.AssociatedPerk, perk))
                {
                    Logger.ErrorS("skills",
                        "Found perk with duplicate PerkDataPrototype {0} - all perks must have" +
                        " a unique prototype, this one will be skipped", perk.AssociatedPerk);
                }
            }
        }
    }
}
