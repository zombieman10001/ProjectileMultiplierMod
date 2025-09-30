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
        // Prevent recursion when we call AddMissile ourselves.
        [ThreadStatic] private static bool _inExtraSpawn;

        // One-time Extreme warning (per session) for player shots.
        private static bool _extremeWarnShown;

        private static MethodInfo _addMissileAuxMI;
        private static bool _methodLookupTried;

        private static bool EnsureMethod()
        {
            if (_addMissileAuxMI != null) return true;
            if (_methodLookupTried) return false;
            _methodLookupTried = true;
            try
            {
                _addMissileAuxMI = typeof(Mission)
                    .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .FirstOrDefault(m => m.Name == "AddMissileAux" && m.GetParameters().Length == 15);
                if (_addMissileAuxMI == null)
                {
                    InformationManager.DisplayMessage(new InformationMessage("[Projectile Multiplier] Could not locate AddMissileAux via reflection", Colors.Yellow));
                }
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage("[Projectile Multiplier] Reflection error: " + ex.Message, Colors.Yellow));
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
            int __result
        )
        {
            if (_inExtraSpawn) return;
            if (isPrediction) return;        // don’t multiply prediction rays
            if (shooterAgent == null) return;
            if (__result <= 0) return;       // original failed

            var s = Settings.Instance;
            bool isPlayer = shooterAgent.IsPlayerControlled;

            // Show a one-time red warning if player Extreme is on.
            if (isPlayer && s.ShowExtremeWarning && s.PlayerUseExtreme && !_extremeWarnShown)
            {
                InformationManager.DisplayMessage(
                    new InformationMessage("[Projectile Multiplier] EXTREME multiplier active", Colors.Red)
                );
                _extremeWarnShown = true;
            }

            // Decide desired multiplier
            int desired = isPlayer ? s.EffectivePlayer : s.EffectiveNpc;
            if (!isPlayer && s.NpcHardCap > 0)
                desired = Math.Min(desired, s.NpcHardCap);

            int extras = Math.Max(0, desired - 1);
            if (extras == 0) return;

            // Spread
            bool slightRandom = s.SlightRandomSpread;
            var rng = Random.Shared;

            if (!EnsureMethod()) return; // can't spawn extras

            _inExtraSpawn = true;
            try
            {
                for (int i = 0; i < extras; i++)
                {
                    Vec3 pos = position;
                    Vec3 dir = direction;
                    Mat3 orient = orientation;
                    GameEntity extraEntity = null;

                    if (slightRandom)
                    {
                        const float jitter = 0.013f; // ~0.75°
                        dir.x += (float)((rng.NextDouble() - 0.5) * jitter);
                        dir.y += (float)((rng.NextDouble() - 0.5) * jitter);
                        dir.z += (float)((rng.NextDouble() - 0.5) * jitter);
                        dir.Normalize();
                    }

                    // Build argument array (15 parameters). Order must match original method.
                    object[] args = new object[]
                    {
                        -1,                 // forcedMissileIndex for extra
                        false,              // isPrediction
                        shooterAgent,       // shooterAgent
                        weaponData,         // in WeaponData (passed by value OK)
                        weaponStatsData,    // WeaponStatsData[]
                        damageBonus,        // damageBonus
                        pos,                // ref Vec3 position
                        dir,                // ref Vec3 direction
                        orient,             // ref Mat3 orientation
                        baseSpeed,          // baseSpeed
                        speed,              // speed
                        addRigidBody,       // addRigidBody
                        gameEntityToIgnore, // gameEntityToIgnore
                        isPrimaryWeaponShot,// isPrimaryWeaponShot
                        extraEntity         // ref GameEntity missileEntity
                    };

                    try
                    {
                        _addMissileAuxMI.Invoke(__instance, args);
                    }
                    catch (Exception ex)
                    {
                        InformationManager.DisplayMessage(new InformationMessage("[Projectile Multiplier] Error spawning extra: " + ex.Message, Colors.Red));
                        break; // abort remaining extras to avoid spam
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
