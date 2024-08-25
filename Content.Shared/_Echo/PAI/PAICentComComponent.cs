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
    public EntProtoId? ElectrocuteActionID = "ActionPAIElectrocute";

    [DataField, AutoNetworkedField]
    public EntityUid? ElectrocuteAction;

    /// <summary>
    /// Sound played when electrocuting someone.
    /// </summary>
    [DataField]
    public SoundSpecifier Sound = new SoundCollectionSpecifier("sparks");
}

/// <summary>
/// Fired when a pAI player presses the Electrocute button.
/// </summary>
public sealed partial class PAIElectrocuteActionEvent : InstantActionEvent
{
}

