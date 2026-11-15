using System;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using Lotus;
using Lotus.Addons;
using VentLib;
using VentLib.Logging;

namespace SampleLotusAddon;


// This file allows for your addon to be loaded from the BepInEx plugin folder. It isn't required, but can solve headaches when trying to figure out why a user's install of your addon is not working if-
// -they put it in the wrong folder. This also allows for the possibility of your addon being able to be listed on marketplaces for mods, if you are interested in that.
// That being said, your addon will still require Lotus to be installed.

// You should not have to modify anything in this file, and I really wouldn't recommend it.
[BepInAutoPlugin($"com.{SampleAddon.AddonAuthor}.{SampleAddon.AddonName}", SampleAddon.AddonName, SampleAddon.AddonVersion)]
[BepInDependency(Vents.Id)]
[BepInDependency(ProjectLotus.Id)]
public partial class BepInExAddonAdapter : BasePlugin
{
    private static readonly StandardLogger log = LoggerFactory.GetLogger<StandardLogger>(typeof(BepInExAddonAdapter));
    private LotusAddon addon;
    
    public const string AddonDLLName = "SampleLotusAddon.dll";

    public override void Load()
    {
        try
        {
            log.Info($"Loading {SampleAddon.AddonName} (from BepInEx plugin)");
            
            if (AddonManager.Addons.Any(x => x.Version.ToSimpleName() == SampleAddon.AddonName)) // addon loaded from 
            {
                log.Warn($"{SampleAddon.AddonName} is already loaded. Skipping.");
                return;
            }
            
            addon = (LotusAddon)Activator.CreateInstance(typeof(SampleAddon));
            if (addon == null) throw new Exception($"Failed to create {typeof(SampleAddon).FullName} instance");
            Vents.Register(Assembly.GetExecutingAssembly());
            AddonManager.Addons.Add(addon);
            addon.Initialize();
        }
        catch (Exception e)
        {
            log.Exception($"Failed to load {SampleAddon.AddonName}", e);
            AddonManager.FailedAddons.Add(SampleAddon.AddonName);
            return;
        }

        log.Info($"{SampleAddon.AddonName} loaded. Running post-initialize.");
        addon.PostInitialize(AddonManager.Addons);
    }
}