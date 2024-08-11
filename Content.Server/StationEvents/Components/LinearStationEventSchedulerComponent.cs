namespace Content.Server.StationEvents.Components;

/// <summary>
///     Echo Station: Introduce Linear event scheduler. See LinearStationEventSchedulerSystem for more info.
/// </summary>
[RegisterComponent, Access(typeof(LinearStationEventSchedulerSystem))]
public sealed partial class LinearStationEventSchedulerComponent : Component
{

    [DataField("startTime"), ViewVariables(VVAccess.ReadWrite)]
    public float StartTime;

    [DataField("endTime"), ViewVariables(VVAccess.ReadWrite)]
    public float EndTime;

    [DataField("startMultiplier"), ViewVariables(VVAccess.ReadWrite)]
    public float StartMultiplier;

    [DataField("EndMultiplier"), ViewVariables(VVAccess.ReadWrite)]
    public float EndMultiplier;

    [DataField("timeUntilNextEvent"), ViewVariables(VVAccess.ReadWrite)]
    public float TimeUntilNextEvent;

}
