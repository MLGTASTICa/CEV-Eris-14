using Robust.Client.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.ViewVariables;
using Content.Shared.Skills;


namespace Content.Client.Skills
{
    public sealed class SkillsMenuBoundUserInterface : BoundUserInterface
    {
        [ViewVariables] private SkillsMenuWindow? _menu;

        public SkillsMenuBoundUserInterface(ClientUserInterfaceComponent owner, object uiKey) : base(owner, uiKey)
        {
            SendMessage(new SkillSyncRequestMessage());
        }

        protected override void Open()
        {
            base.Open();

            _menu = new SkillsMenuWindow(this);
            if (State != null)
                UpdateState(State);

            _menu.OnClose += Close;
            _menu.OpenCentered();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
                return;

            _menu?.Dispose();
        }

        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);
            if (_menu == null || state is not SkillSyncDataState cast)
                return;

            _menu.Populate_PerkTab(cast.Perks);
            _menu.Populate_SkillsTab(cast.Skills);
        }

    }
}




