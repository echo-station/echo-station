using Content.Server.GameTicking;
using Content.Server.GameTicking.Components;
using Content.Server.GameTicking.Rules;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.StationEvents.Components;
using Content.Server.StationEvents.Events;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Random;

namespace Content.Server.StationEvents;

/// <summary>
///     Echo Station: Adds a linear event scheduler with high configurability to replace the standard "ramping"
///     event scheduler. That one has many things hardcoded, including min/max event times, and a very
///     aggressive exponential curve. The linear schedule acts as advertised.
///
///     Min/max event times are multiplied by the current multiplier. The multiplier is scaled linearly
///     from StartMultiplier to EndMultiplier, starting from StartTime. If StartTime is nonzero, all time
///     before that point will use the StartMultiplier. The same principle applies for exceeding the EndTime.
/// </summary>
public sealed class LinearStationEventSchedulerSystem : GameRuleSystem<LinearStationEventSchedulerComponent>
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly EventManagerSystem _event = default!;
    [Dependency] private readonly GameTicker _gameTicker = default!;

    public float GetLinearModifier(EntityUid uid, LinearStationEventSchedulerComponent component)
    {
        var roundTime = (float) _gameTicker.RoundDuration().TotalSeconds;
        if (roundTime < component.StartTime)
            return component.StartMultiplier;
        if (roundTime > component.EndTime)
            return component.EndMultiplier;

        // Determine how far we've progressed into the ramping.
        var relativeRoundTime = roundTime - component.StartTime;
        var relativeEndTime = component.EndTime - component.StartTime;
        var ramp = relativeRoundTime / relativeEndTime; // Outputs in range: [0.0, 1.0]

        // Adjust for minimum multiplier. We invert the ramp since we want to start at a high
        // value and decrease over time, not the other way around.
        var rampAdjusted = component.EndMultiplier + (1 - ramp) * (1 - component.EndMultiplier);

        return rampAdjusted;
    }

    protected override void Started(EntityUid uid, LinearStationEventSchedulerComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        component.StartTime = _cfg.GetCVar(CCVars.EventsLinearStartTime) * 60f; // Convert minutes to seconds
        component.EndTime = _cfg.GetCVar(CCVars.EventsLinearEndTime) * 60f; // Convert minutes to seconds

        component.StartMultiplier = _cfg.GetCVar(CCVars.EventsLinearStartMultiplier);
        component.EndMultiplier = _cfg.GetCVar(CCVars.EventsLinearEndMultiplier);

        PickNextEventTime(uid, component);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_event.EventsEnabled)
            return;

        var query = EntityQueryEnumerator<LinearStationEventSchedulerComponent, GameRuleComponent>();
        while (query.MoveNext(out var uid, out var scheduler, out var gameRule))
        {
            if (!GameTicker.IsGameRuleActive(uid, gameRule))
                return;

            if (scheduler.TimeUntilNextEvent > 0f)
            {
                scheduler.TimeUntilNextEvent -= frameTime;
                return;
            }

            PickNextEventTime(uid, scheduler);
            _event.RunRandomEvent();
        }
    }

    private void PickNextEventTime(EntityUid uid, LinearStationEventSchedulerComponent component)
    {
        var mod = GetLinearModifier(uid, component);
        // Echo Station: Make min/max configurable
        var baselineMinTime = _cfg.GetCVar(CCVars.EventsLinearBaselineMinTime);
        var baselineMaxTime = _cfg.GetCVar(CCVars.EventsLinearBaselineMaxTime);

        // Echo Station: Use configured min/max
        component.TimeUntilNextEvent = _random.NextFloat(baselineMinTime * mod, baselineMaxTime * mod);
    }
}
