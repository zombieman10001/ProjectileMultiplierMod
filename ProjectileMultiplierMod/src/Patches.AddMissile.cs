using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ProjectileMultiplierMod
{
    [HarmonyPatch(typeof(Mission))]
    internal static class Patch_AddMissileAux
    {
        [ThreadStatic] 
        private static bool _inExtraSpawn;

        private static bool _extremeWarnShown;
        private static MethodInfo _addMissileAuxMI;
        private static bool _methodLookupTried;

        private static bool EnsureMethod()
        {
            if (_addMissileAuxMI != null) 
                return true;

            if (_methodLookupTried) 
                return false;

            _methodLookupTried = true;

            try
            {
                _addMissileAuxMI = typeof(Mission)
                    .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .FirstOrDefault(m => m.Name == "AddMissileAux" && m.GetParameters().Length == 15);

                if (_addMissileAuxMI == null)
                {
                    InformationManager.DisplayMessage(
                        new InformationMessage("[Projectile Multiplier] Could not locate AddMissileAux via reflection", Colors.Yellow));
                }
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(
                    new InformationMessage("[Projectile Multiplier] Reflection error: " + ex.Message, Colors.Yellow));
            }

            return _addMissileAuxMI != null;
        }

        [HarmonyPostfix]
        [HarmonyPatch("AddMissileAux")]
        private static void Postfix(
            Mission __instance,
            int forcedMissileIndex,
            bool isPrediction,
            Agent shooterAgent,
            in WeaponData weaponData,
            WeaponStatsData[] weaponStatsData,
            float damageBonus,
            ref Vec3 position,
            ref Vec3 direction,
            ref Mat3 orientation,
            float baseSpeed,
            float speed,
            bool addRigidBody,
            GameEntity gameEntityToIgnore,
            bool isPrimaryWeaponShot,
            ref GameEntity missileEntity,
            int __result)
        {
            try
            {
                if (_inExtraSpawn || isPrediction || shooterAgent == null || __result <= 0)
                    return;

                var settings = Settings.Instance;
                if (settings == null)
                    return;

                bool isPlayer = shooterAgent.IsPlayerControlled;

                if (isPlayer && settings.ShowExtremeWarning && settings.PlayerUseExtreme && !_extremeWarnShown)
                {
                    InformationManager.DisplayMessage(
                        new InformationMessage("[Projectile Multiplier] EXTREME multiplier active", Colors.Red));
                    _extremeWarnShown = true;
                }

                int desired = isPlayer ? settings.EffectivePlayer : settings.EffectiveNpc;
                if (!isPlayer && settings.NpcHardCap > 0)
                    desired = Math.Min(desired, settings.NpcHardCap);

                int extras = Math.Max(0, desired - 1);
                if (extras == 0)
                    return;

                if (!EnsureMethod())
                    return;

                SpawnExtraMissiles(__instance, extras, settings.SlightRandomSpread, 
                    shooterAgent, weaponData, weaponStatsData, damageBonus,
                    position, direction, orientation, baseSpeed, speed, 
                    addRigidBody, gameEntityToIgnore, isPrimaryWeaponShot);
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(
                    new InformationMessage($"[Projectile Multiplier] Unexpected error: {ex.Message}", Colors.Red));
            }
        }

        private static void SpawnExtraMissiles(
            Mission instance,
            int extras,
            bool slightRandom,
            Agent shooterAgent,
            WeaponData weaponData,
            WeaponStatsData[] weaponStatsData,
            float damageBonus,
            Vec3 position,
            Vec3 direction,
            Mat3 orientation,
            float baseSpeed,
            float speed,
            bool addRigidBody,
            GameEntity gameEntityToIgnore,
            bool isPrimaryWeaponShot)
        {
            _inExtraSpawn = true;
            try
            {
                var rng = Random.Shared;

                for (int i = 0; i < extras; i++)
                {
                    Vec3 pos = position;
                    Vec3 dir = direction;
                    Mat3 orient = orientation;

                    if (slightRandom)
                    {
                        const float jitter = 0.013f;
                        dir.x += (float)((rng.NextDouble() - 0.5) * jitter);
                        dir.y += (float)((rng.NextDouble() - 0.5) * jitter);
                        dir.z += (float)((rng.NextDouble() - 0.5) * jitter);
                        dir.Normalize();
                    }

                    object[] args = new object[]
                    {
                        -1,
                        false,
                        shooterAgent,
                        weaponData,
                        weaponStatsData,
                        damageBonus,
                        pos,
                        dir,
                        orient,
                        baseSpeed,
                        speed,
                        addRigidBody,
                        gameEntityToIgnore,
                        isPrimaryWeaponShot,
                        null
                    };

                    try
                    {
                        _addMissileAuxMI.Invoke(instance, args);
                    }
                    catch (Exception ex)
                    {
                        InformationManager.DisplayMessage(
                            new InformationMessage($"[Projectile Multiplier] Error spawning projectile: {ex.Message}", Colors.Red));
                        break;
                    }
                }
            }
            finally
            {
                _inExtraSpawn = false;
            }
        }
    }
}
