using SampleRoleAddon.Roles;
using Lotus.Addons;
using Lotus.GameModes.Standard;
using SampleRoleAddon.Version;
using Lotus.Roles;
using System.Collections.Generic;

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
    }

    public override string Name { get;} = "Sample Role Addon";

    public override VentLib.Version.Version Version { get;} = new SampleLotusAddonVersion();
}


