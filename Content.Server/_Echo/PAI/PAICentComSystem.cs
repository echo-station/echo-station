using Content.Shared._Echo.PAI;
using Content.Server.Actions;
using Robust.Shared.Audio.Systems;
using Content.Server.Electrocution;
using Robust.Server.GameObjects;

namespace Content.Server._Echo.PAI;

public sealed class PAICentComSystem : SharedPAICentComSystem
{
    [Dependency] private readonly ActionsSystem _actionsSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly ElectrocutionSystem _electrocution = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly TransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PAICentComComponent, PAIElectrocuteActionEvent>(OnElectrocuteAction);
    }

    /// <summary>
    /// Add ability for Central Command pAI to electrocute whoever is near them, once per 20 seconds.
    ///
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="component"></param>
    /// <param name="args"></param>
    private void OnElectrocuteAction(EntityUid uid, PAICentComComponent component, PAIElectrocuteActionEvent args)
    {
        if (args.Handled)
            return;

        foreach (var targetUid in _lookup.GetEntitiesInRange(_transform.GetMapCoordinates(uid), 0.2f))
            _electrocution.TryDoElectrocution(targetUid, uid, 5, TimeSpan.FromSeconds(3), false);
        _audio.PlayPvs(component.Sound, uid);

        args.Handled = true;
    }
}
