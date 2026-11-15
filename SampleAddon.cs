using System.Collections.Generic;
using Lotus.Addons;
using Lotus.GameModes;
using Lotus.GameModes.Normal.Standard;
using Lotus.Roles;
using SampleLotusAddon.Gamemodes.BombTag;
using SampleLotusAddon.Gamemodes.BombTag.Options;
using SampleLotusAddon.Version;
using SampleLotusAddon.Roles.Standard;

namespace SampleLotusAddon;

public class SampleAddon: LotusAddon
{
    // Some metadata about your addon, these fields are used in different parts of this addon.
    public const string AddonName = "SampleLotusAddon"; // The name of your addon.
    public const string AddonVersion = "1.0.0"; // The version of your addon.
    public const string AddonAuthor = "YourName"; // Your name as the author of the addon.
    
    // This is the main entry point for your addon, where stuff like gamemodes, roles, harmony patching, etc should be registered.
    // If you want to check for potential incompatibilities with other addons, you should do that in PostInitialize, as all addons will have been loaded by then.
    public override void Initialize()
    {
        // Create instances first
        List<CustomRole> allRoles = new List<CustomRole>() { new CrewCrew() };

        // Register roles
        ExportCustomRoles(allRoles, typeof(NormalStandardGameMode));
        
        // Export gamemodes
        ExportGameModes(new List<IGameMode>() { new BombTagGamemode() });
        _ = new BombTagOptionHolder();
    }

    public override string Name { get; } = AddonName;

    public override VentLib.Version.Version Version { get; } = new SampleLotusAddonVersion();
}


