using Content.Shared.Body.Systems;
using Content.Shared.Movement.Systems;

namespace Content.Shared.Traits.Assorted
{
    /// <summary>
    /// Applies low mobility to entities with a LowMobilityComponent.
    /// </summary>
    public sealed class LowMobilitySystem : EntitySystem
    {
        [Dependency] private readonly MovementSpeedModifierSystem _movementSpeedModifierSystem = default!;
        [Dependency] private readonly SharedBodySystem _bodySystem = default!;

        /// <summary>
        /// Registers for the startup and shutdown events thrown by the LowMobilityComponent.
        /// Note that the base class automatically unsubscribes from events on shutdown.
        /// </summary>
        public override void Initialize()
        {
            SubscribeLocalEvent<LowMobilityComponent, ComponentStartup>((uid, comp, _) => OnStartup(uid, comp));
            SubscribeLocalEvent<LowMobilityComponent, ComponentShutdown>((uid, _, _) => OnShutdown(uid));
        }

        /// <summary>
        /// When the component is added, modify the base move speed of the player to that which is defined in the component.
        /// </summary>
        /// <param name="uid">Unique identifier of the entity to modify the base move speed of.</param>
        /// <param name="component">Component containing the adjusted base walk speed and sprint speed.</param>
        private void OnStartup(EntityUid uid, LowMobilityComponent component)
        {
            _movementSpeedModifierSystem.ChangeBaseSpeed(uid, component.LowMobilityBaseWalkSpeed, component.LowMobilityBaseSprintSpeed, 20);
        }

        /// <summary>
        /// When the component is removed, reset the base move speed of the player based on the body system.
        /// </summary>
        /// <param name="uid">Unique identifier of the entity to recalculate the base move speed of.</param>
        private void OnShutdown(EntityUid uid)
        {
            _bodySystem.UpdateMovementSpeed(uid);
        }
    }
}
