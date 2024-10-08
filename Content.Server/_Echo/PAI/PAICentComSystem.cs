using Content.Shared._Echo.PAI;
using Robust.Shared.Audio.Systems;
using Content.Server.Electrocution;
using Robust.Server.GameObjects;
using Content.Shared.Access.Components;
using Content.Server.Access.Systems;
using Robust.Shared.Timing;

namespace Content.Server._Echo.PAI;

public sealed class PAICentComSystem : SharedPAICentComSystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly ElectrocutionSystem _electrocution = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly TransformSystem _transform = default!;
    [Dependency] private readonly AccessSystem _access = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PAICentComComponent, PAIElectrocuteActionEvent>(OnElectrocuteAction);
        SubscribeLocalEvent<PAICentComComponent, PAIUnlockActionEvent>(OnUnlockAction);
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
        _audio.PlayPvs(component.ElectrocutionSound, uid);

        args.Handled = true;
    }

    /// <summary>
    /// Add ability for Central Command pAI to give themselves All Access for 30 seconds.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="component"></param>
    /// <param name="args"></param>
    private void OnUnlockAction(EntityUid uid, PAICentComComponent component, PAIUnlockActionEvent args)
    {
        if (args.Handled)
            return;

        if (TryComp<AccessComponent>(uid, out var accessComponent))
            _access.SetAccessEnabled(uid, true, accessComponent);

        component.LockTime = _gameTiming.CurTime + TimeSpan.FromSeconds(component.AllAccessTime);

        _audio.PlayPvs(component.AccessSound, uid);

        args.Handled = true;
    }

    /// <summary>
    /// For when the pAI loses All Access.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="component"></param>
    private void Lock(EntityUid uid, PAICentComComponent component)
    {
        component.LockTime = null;
        if (TryComp<AccessComponent>(uid, out var accessComponent))
            _access.SetAccessEnabled(uid, false, accessComponent);
        _audio.PlayPvs(component.NoAccessSound, uid);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_gameTiming.IsFirstTimePredicted)
            return;

        var query = AllEntityQuery<PAICentComComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.LockTime == null || _gameTiming.CurTime < comp.LockTime)
                continue;
            Lock(uid, comp);
        }
    }
}
