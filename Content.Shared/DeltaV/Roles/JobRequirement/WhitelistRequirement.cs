using System.Diagnostics.CodeAnalysis;
using Content.Shared.Preferences;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Roles;

/// <summary>
/// Requires the player be globally whitelisted to play a role.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class WhitelistRequirement : JobRequirement
{
    public override bool Check(IEntityManager entManager,
        IPrototypeManager protoManager,
        HumanoidCharacterProfile? profile,
        IReadOnlyDictionary<string, TimeSpan> playTimes,
        out FormattedMessage reason,
        float roleTimersMultiplier, // Echo
        bool isWhitelisted)
    {
        reason = FormattedMessage.FromMarkupPermissive(Loc.GetString("role-server-whitelisted")); // Echo

        if (isWhitelisted)
            return true;

        reason = FormattedMessage.FromMarkupPermissive(Loc.GetString("role-not-server-whitelisted")); // Echo
        return false;
    }
}
