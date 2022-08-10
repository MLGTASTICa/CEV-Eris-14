using Robust.Client.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.ViewVariables;
using Content.Shared.Skills;


namespace Content.Client.Skills
{
    public sealed class SkillsMenuBoundUserInterface : BoundUserInterface
    {
        [ViewVariables] private SkillsMenu? _menu;
        public SharedSkillsComponent? SkillHolder { get; private set; }

        /*
        public SkillsMenuBoundUserInterface(ClientUserInterfaceComponent owner, object uiKey) : base(owner, uiKey)
        {
            SendMessage(new InventorySyncRequestMessage());
        }
        */
        protected override void Open()
        {
            base.Open();

            var entMan = IoCManager.Resolve<IEntityManager>();
            if (!entMan.TryGetComponent(Owner.Owner, out SharedSkillsComponent? skill))
            {
                return;
            }

            SkillHolder = skill;

            _menu = new SkillMenu(this) { Title = "bbazinga" };
            _menu.Populate(skill.AcquiredPerks);

            _menu.OnClose += Close;
            _menu.OpenCentered();
        }

        public void Eject(InventoryType type, string id)
        {
            SendMessage(new VendingMachineEjectMessage(type, id));
        }

        protected override void ReceiveMessage(BoundUserInterfaceMessage message)
        {
            switch (message)
            {
                case VendingMachineInventoryMessage msg:
                    _menu?.Populate(msg.Inventory);
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
                return;

            _menu?.Dispose();
        }
    }
}




