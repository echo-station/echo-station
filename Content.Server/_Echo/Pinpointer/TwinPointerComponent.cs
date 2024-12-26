using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Server._Echo.Pinpointer;

/// <summary>
/// Displays a sprite on the item that points towards the target component.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
[Access(typeof(TwinPointerSystem))]
public sealed partial class TwinPointerComponent : Component
{
}
