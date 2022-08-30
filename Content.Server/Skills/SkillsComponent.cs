using Content.Shared.Skills;
using Content.Server.UserInterface;
using Robust.Server.GameObjects;


namespace Content.Server.Skills
{
    [RegisterComponent()]
    public sealed class SkillsComponent : SharedSkillsComponent
    {

        [ViewVariables] public BoundUserInterface? UserInterface => Owner.GetUIOrNull(SkillMenuUiKey.Key);

    }
}
