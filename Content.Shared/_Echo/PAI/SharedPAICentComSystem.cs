using Content.Shared.Actions;

namespace Content.Shared._Echo.PAI
{
    /// <summary>
    /// Additional actions that a Central Command pAI can perform.
    /// </summary>
    public abstract class SharedPAICentComSystem : EntitySystem
    {
        [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<PAICentComComponent, MapInitEvent>(OnMapInit);
            SubscribeLocalEvent<PAICentComComponent, ComponentShutdown>(OnShutdown);
        }

        private void OnMapInit(EntityUid uid, PAICentComComponent component, MapInitEvent args)
        {
            _actionsSystem.AddAction(uid, ref component.ElectrocuteAction, component.ElectrocuteActionID);
            _actionsSystem.AddAction(uid, ref component.UnlockAction, component.UnlockActionID);
        }

        private void OnShutdown(EntityUid uid, PAICentComComponent component, ComponentShutdown args)
        {
            _actionsSystem.RemoveAction(uid, component.ElectrocuteAction);
            _actionsSystem.RemoveAction(uid, component.UnlockAction);
        }
    }
}

