using flanne.PowerupSystem;
using HarmonyLib;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ItrsTweaks
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(ProjectileOnShoot), nameof(ProjectileOnShoot.Attack))]
    public static class ProjectileOnShootAttackPatch
    {
        public static bool Prefix(ProjectileOnShoot __instance, float ___inaccuracy, int ___numProjectiles)
        {
            if (!MainClass.RandomShotsAffectOnShoot) return true;
            if (!DumpAmmoPassiveShootRandomPatch.CurrentlyRandomShooting.HasValue) return true;

            Vector2 v = DumpAmmoPassiveShootRandomPatch.CurrentlyRandomShooting.Value.normalized;
            float num = -1f * ___inaccuracy / 2f;
            float max = -1f * num;
            if (___numProjectiles > 1)
            {
                for (int i = 0; i < ___numProjectiles; i++)
                {
                    float degrees = num + (float)i / (float)(___numProjectiles - 1) * ___inaccuracy;
                    Vector2 direction = v.Rotate(degrees);
                    Traverse.Create(__instance).Method("SpawnProjectile", direction).GetValue(direction);
                }
            }
            else
            {
                Vector2 direction2 = v.Rotate(Random.Range(num, max));
                Traverse.Create(__instance).Method("SpawnProjectile", direction2).GetValue(direction2);
            }

            return false;
        }
    }
}
