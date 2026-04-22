using Lotus.Addons;
using Lotus.GameModes.Normal.Standard;
using SampleRoleAddon.Version;
using Lotus.Roles;
using System.Collections.Generic;
using SampleRoleAddon.Roles.Standard;
using SampleRoleAddon.Gamemodes.BombTag;
using SampleRoleAddon.Gamemodes.BombTag.Options;
using Lotus.GameModes;

namespace SampleRoleAddon;

public class SampleRoleAddon: LotusAddon
{
    public override void Initialize()
    {
        // Create instances first
        List<CustomRole> allRoles = new List<CustomRole>() { new CrewCrew() };

        // Register roles
        ExportCustomRoles(allRoles, typeof(NormalStandardGameMode));
        
        // Export gamemode
        ExportGameModes(new List<IGameMode>() { new BombTagGamemode() });
        _ = new BombTagOptionHolder();
    }

    public override string Name { get; } = "Sample Role Addon";

    public override VentLib.Version.Version Version { get; } = new SampleLotusAddonVersion();
}


