using Content.Server.Chat.Systems;
using Content.Shared.Chat;
using Content.Shared.Radio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Server.Radio.Components;

/// <summary>
///     Echo Station: Makes the ActiveRadioComponent and IntrinsicRadioReceiverComponent update when encryption keys change.
///     This is necessary since you basically have to "manually" update the ActiveRadioComponent. For example, the
///     HeadsetSystem does this manually, as does the RadioImplantSystem.
/// </summary>
[RegisterComponent]
public sealed partial class IntrinsicRadioFromKeysComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("permanentChannels", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<RadioChannelPrototype>))]
    public HashSet<string> PermanentChannels = new();
}
