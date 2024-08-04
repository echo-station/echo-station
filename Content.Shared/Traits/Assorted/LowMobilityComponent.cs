using Content.Shared.Movement.Components;
using Robust.Shared.GameStates;

namespace Content.Shared.Traits.Assorted;

/// <summary>
/// Determines the move speed that should be given to entities with low mobility.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(LowMobilitySystem))]
public sealed partial class LowMobilityComponent : Component
{
    /// <summary>
    /// Determines what the low mobility walk speed should be.
    /// </summary>
    [DataField("LowMobilityBaseWalkSpeed"), ViewVariables(VVAccess.ReadWrite)]
    public float LowMobilityBaseWalkSpeed = MovementSpeedModifierComponent.DefaultBaseWalkSpeed * 0.2f;

    /// <summary>
    /// Determines what the low mobility base sprint speed should be.
    /// </summary>
    [DataField("LowMobilityBaseSprintSpeed"), ViewVariables(VVAccess.ReadWrite)]
    public float LowMobilityBaseSprintSpeed = MovementSpeedModifierComponent.DefaultBaseSprintSpeed * 0.2f;
}
