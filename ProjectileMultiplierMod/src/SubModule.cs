using HarmonyLib;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ProjectileMultiplierMod
{
    public class SubModule : MBSubModuleBase
    {
        private Harmony _harmony;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            _harmony = new Harmony("ProjectileMultiplierMod.Harmony");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        protected override void OnGameStart(Game game, IGameStarter starterObject)
        {
            base.OnGameStart(game, starterObject);
            InformationManager.DisplayMessage(new InformationMessage("[ProjectileMultiplier] Loaded"));
        }

        protected override void OnSubModuleUnloaded()
        {
            _harmony?.UnpatchAll(_harmony.Id);
            base.OnSubModuleUnloaded();
        }
    }
}
