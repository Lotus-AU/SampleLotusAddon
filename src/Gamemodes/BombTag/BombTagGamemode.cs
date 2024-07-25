using System.Collections.Generic;
using System.Linq;
using Lotus.API.Odyssey;
using Lotus.API.Player;
using Lotus.API.Reactive;
using Lotus.API.Reactive.HookEvents;
using Lotus.GameModes;
using Lotus.Roles;
using Lotus.Victory;
using VentLib.Logging;
using VentLib.Options.UI.Tabs;
using VentLib.Utilities.Extensions;
using SampleRoleAddon.Gamemodes.BombTag.Options;
using Lotus;
using Lotus.Extensions;
using Lotus.GUI.Name.Components;
using SampleRoleAddon.Gamemodes.BombTag.WinCons;

namespace SampleRoleAddon.Gamemodes.BombTag;

// Gamemodes are a LOT more complicated than just roles, so it easy to mess up here.
public class BombTagGamemode: GameMode
{
    internal static MainSettingTab BombTagOptions = new("BOMB TAG SETTINGS", "Modify the Bomb Tag settings here!");
    internal static IEnumerable<GameOptionTab> NoTabs = new List<GameOptionTab>();
    private const string BombTagGamemodeHookKey = nameof(BombTagGamemodeHookKey);
    public static BombTagGamemode Instance;

    public override string Name { get; set; } = "Bomb Tag";
    public override BombTagRoleOperations RoleOperations { get; }
    public override BombTagRoleManager RoleManager { get; }
    public new MatchData MatchData { get; private set; }

    public BombTagGamemode(): base()
    {
        Instance = this;
        MatchData = new();

        RoleOperations = new(this);
        RoleManager = new();
    }

    public override void Setup()
    {
        MatchData = new MatchData();
    }
    public override void Activate()
    {
        Hooks.PlayerHooks.PlayerDeathHook.Bind(BombTagGamemodeHookKey, ShowInformationToGhost, priority: Lotus.API.Priority.VeryLow);
    }

    public override void Deactivate()
    {
        Hooks.UnbindAll(BombTagGamemodeHookKey);
    }

    // If you wanted to add roles to the options, you can add tabs here. But we dont have any role tabs.
    public override IEnumerable<GameOptionTab> EnabledTabs() => NoTabs;
    
    // The MainSettingTab tab replaces the Game Presets button with your custom settings.
    public override MainSettingTab MainTab() => BombTagOptions;

    // This function actually assigns the role to the player.
    // Please never set sendToClient as true. It will cause a lot of issues in the long run and some glitches might occur.
    // It's still here for decaprecation.
    public override void Assign(PlayerControl player, CustomRole role, bool addAsMainRole = true, bool sendToClient = false) => RoleOperations.Assign(role, player, addAsMainRole, sendToClient);

    // This function runs with RoleManager.SelectRoles, and is used to assign a role to every player.
    // If a player does not have a role they will be given a FallbackRole that essentially means no role.
    public override void AssignRoles(List<PlayerControl> players)
    {
        // IMPORTANT!! Create a new list since we pass the old one to base.
        List<PlayerControl> unassignedPlayers = new(players);
        int seekersLeft = BombTagOptionHolder.BombCount;
        // Assign Seekers
        while (seekersLeft > 0)
        {
            if (unassignedPlayers.Count == 0) break; // prevent error by breaking if we have zero players

            // Gets a random element from a list and removes it from the list.
            PlayerControl seeker = unassignedPlayers.PopRandom();
            seekersLeft -= 1;
            Assign(seeker, RoleManager.RoleHolder.Static.HasBomb); // use role instance to assign role
            if (seekersLeft == 0) break;
        }
        // Assign Hiders
        while (unassignedPlayers.Count > 0)
        {
            PlayerControl hider = unassignedPlayers.PopRandom();
            Assign(hider, RoleManager.RoleHolder.Static.HasNoBomb); // use role instance to assign role
        }
        if (ProjectLotus.AdvancedRoleAssignment) Players.GetAllPlayers().Sorted(p => p.IsHost() ? 0 : 1).ForEach(p => p.RpcSetRole(AmongUs.GameOptions.RoleTypes.Crewmate, true));
        else Players.GetAllPlayers().Sorted(p => p.IsHost() ? 0 : 1).ForEach(p => p.PrimaryRole().Assign());
        base.AssignRoles(players); // let base finish everything.
    }

    // Add win cons to the WinDelegate. With no win cons, the game would never end.
    public override void SetupWinConditions(WinDelegate winDelegate)
    {
        winDelegate.AddWinCondition(new BombTagWinCondition());
    }

    public static void ShowInformationToGhost(PlayerDeathHookEvent hookEvent)
    {
        PlayerControl player = hookEvent.Player;
        ShowInformationToGhost(player);
    }

    // Project Lotus doesn't automatically show all roles to every player.
    // You might think this kind of stupid, but it's actually a lot more power to you as a developer.
    // You can prevent certain players from seeing roles if they have an after-death type role.
    public static void ShowInformationToGhost(PlayerControl player)
    {
        if (player == null) return;

        StaticLogger.Trace($"Showing all name components to ghost {player.name}");

        Players.GetAllPlayers().Where(p => p.PlayerId != player.PlayerId)
            .SelectMany(p => p.NameModel().ComponentHolders())
            .ForEach(holders =>
                {
                    if (holders is RoleComponent)
                    {
                        holders.AddListener(component => component.AddViewer(player));
                        holders.Components().ForEach(components => components.AddViewer(player));
                    }
                }
            );

        player.NameModel().Render(force: true);
    }
}