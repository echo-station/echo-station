using Content.Server.Pinpointer;
using Content.Shared.Pinpointer;

namespace Content.Server._Echo.Pinpointer;

public sealed class TwinPointerSystem : EntitySystem
{
    [Dependency] private readonly SharedPinpointerSystem _pinpointerSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TwinPointerComponent, AfterItemsSpawnEvent>(Handler);
    }

    private void Handler(EntityUid uid, TwinPointerComponent component, AfterItemsSpawnEvent args)
    {
        var left = args.EntityUids[0];
        var right = args.EntityUids[1];
        _pinpointerSystem.SetTarget(left, right);
        _pinpointerSystem.SetTarget(right, left);
    }
}
