using Content.Shared.Actions;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Echo.PAI;

/// <summary>
/// Additional actions that a Central Command pAI can perform.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class PAICentComComponent : Component
{
    public EntProtoId? UnlockActionID = "PAIAllAccess";

    [DataField, AutoNetworkedField]
    public EntityUid? UnlockAction;
    public EntProtoId? ElectrocuteActionID = "ActionPAIElectrocute";

    [DataField, AutoNetworkedField]
    public EntityUid? ElectrocuteAction;

    /// <summary>
    /// Sound played when electrocuting someone.
    /// </summary>
    [DataField]
    public SoundSpecifier ElectrocutionSound = new SoundCollectionSpecifier("sparks");

    /// <summary>
    /// Sound played when turning on All Access.
    /// </summary>
    [DataField]
    public SoundSpecifier AccessSound = new SoundPathSpecifier("/Audio/Effects/RingtoneNotes/a.ogg");

    /// <summary>
    /// Sound played when All Access turns off.
    /// </summary>
    [DataField]
    public SoundSpecifier NoAccessSound = new SoundPathSpecifier("/Audio/Effects/RingtoneNotes/c.ogg");

    /// <summary>
    /// Time in seconds that the pAI has All Access when using their All Access button.
    /// Beware, if you change this here, you must also change it in the prototype useDelay and description.
    /// </summary>
    [DataField]
    public int AllAccessTime = 30;

    /// <summary>
    /// From which point in game time the pAI no longer has All Access.
    /// </summary>
    public TimeSpan? LockTime;
}

/// <summary>
/// Fired when a pAI player presses the Electrocute button.
/// </summary>
public sealed partial class PAIElectrocuteActionEvent : InstantActionEvent
{
}

/// <summary>
/// Fired when a pAI player presses the All Access button.
/// </summary>
public sealed partial class PAIUnlockActionEvent : InstantActionEvent
{
}
