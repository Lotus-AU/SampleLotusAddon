using Lotus.Addons;
using Lotus.GameModes.Standard;
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
        CustomRole crewCrew = new CrewCrew();
        List<CustomRole> allRoles = new List<CustomRole>() { crewCrew };

        // Register roles second
        ExportCustomRoles(allRoles, typeof(StandardGameMode));

        // Add your role to the gamemmode of your choice (Standard in this case.)
        allRoles.ForEach(StandardRoles.AddRole);
        
        // Export gamemode
        ExportGameModes(new List<IGameMode>() { new BombTagGamemode() });
        _ = new BombTagOptionHolder();
    }

    public override string Name { get;} = "Sample Role Addon";

    public override VentLib.Version.Version Version { get;} = new SampleLotusAddonVersion();
}


