using Hazel;

namespace SampleLotusAddon.Version;


/// <summary>
/// Version Representing this Addon
/// </summary>
public class SampleLotusAddonVersion: VentLib.Version.Version
{
    public override VentLib.Version.Version Read(MessageReader reader)
    {
        return new SampleLotusAddonVersion();
    }

    protected override void WriteInfo(MessageWriter writer)
    {
    }

    public override string ToSimpleName()
    {
        return SampleAddon.AddonName;
    }

    public override string ToString() => SampleAddon.AddonVersion;
}