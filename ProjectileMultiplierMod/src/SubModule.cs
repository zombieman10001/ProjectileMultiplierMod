using HarmonyLib;
using System;
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

            try
            {
                _harmony = new Harmony("ProjectileMultiplierMod.Harmony");
                _harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(
                    new InformationMessage($"[Projectile Multiplier] Error loading mod: {ex.Message}", Colors.Red));
            }
        }

        protected override void OnGameStart(Game game, IGameStarter starterObject)
        {
            try
            {
                base.OnGameStart(game, starterObject);
                InformationManager.DisplayMessage(
                    new InformationMessage("[Projectile Multiplier] Loaded", Colors.Green));
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(
                    new InformationMessage($"[Projectile Multiplier] Error on game start: {ex.Message}", Colors.Red));
            }
        }

        protected override void OnSubModuleUnloaded()
        {
            try
            {
                _harmony?.UnpatchAll(_harmony.Id);
            }
            catch { }
            
            base.OnSubModuleUnloaded();
        }
    }
}
