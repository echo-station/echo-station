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
        [Dependency] private readonly IEntityManager _entMan = default!;

        private EntityQuery<LegsParalyzedComponent> _legsParalyzedQuery;
        private EntityQuery<LowMobilityComponent> _lowMobilityQuery;

        /// <summary>
        /// Initializes any necessary functionalities and registers for necessary events.
        /// Note that the base class automatically unsubscribes from events on shutdown.
        /// </summary>
        public override void Initialize()
        {
            _legsParalyzedQuery = _entMan.GetEntityQuery<LegsParalyzedComponent>();
            _lowMobilityQuery = _entMan.GetEntityQuery<LowMobilityComponent>();

            // We need to adjust the base move speed when the low mobility component is added or removed
            SubscribeLocalEvent<LowMobilityComponent, ComponentStartup>(OnStartup);
            SubscribeLocalEvent<LowMobilityComponent, ComponentShutdown>(OnShutdown);
        }

        #region Event Wrappers
        private void OnStartup(EntityUid uid, LowMobilityComponent comp, ComponentStartup args) => SetMoveSpeed(uid, comp);
        private void OnShutdown(EntityUid uid, LowMobilityComponent comp, ComponentShutdown args) => SurrenderMoveSpeed(uid);
        #endregion

        #region Public Update Functions
        public void UpdateMoveSpeed(EntityUid uid, bool ignoreLegsParalyzed = false)
        {
            if (_lowMobilityQuery.TryGetComponent(uid, out var comp))
                SetMoveSpeed(uid, comp, ignoreLegsParalyzed);
        }
        #endregion

        #region Private Update Functions
        /// <summary>
        /// Modify the base move speed of the player to that which is defined in the component.
        /// </summary>
        /// <param name="uid">Unique identifier of the entity to modify the base move speed of.</param>
        /// <param name="component">Component containing the adjusted base walk speed and sprint speed.</param>
        /// <param name="ignoreLegsParalyzed">Whether the LegsParalyzed component of an entity should be ignored (assuming it has one).</param>
        private void SetMoveSpeed(EntityUid uid, LowMobilityComponent component, bool ignoreLegsParalyzed = false)
        {
            if (component is null) return;

            // If legs should be paralyzed and isn't being explicitly ignored, do not set move speed.
            if (!ignoreLegsParalyzed && _legsParalyzedQuery.HasComponent(uid)) return;

            _movementSpeedModifierSystem.ChangeBaseSpeed(uid, component.LowMobilityBaseWalkSpeed, component.LowMobilityBaseSprintSpeed, 20);
        }

        /// <summary>
        /// Reset the base move speed of the player based on the body system.
        /// </summary>
        /// <param name="uid">Unique identifier of the entity to recalculate the base move speed of.</param>
        private void SurrenderMoveSpeed(EntityUid uid)
        {
            // If legs should be paralyzed, do not refresh move speed.
            if (_legsParalyzedQuery.HasComponent(uid)) return;
            _bodySystem.UpdateMovementSpeed(uid);
        }
        #endregion
    }
}
