using flanne;
using flanne.PowerupSystem;
using HarmonyLib;
using UnityEngine;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(ShootBulletsBehind), "OnShoot")]
    public static class ShootBHulletsBehindOnShootPatch
    {
        public static bool Prefix(ShootBulletsBehind __instance, float ___spread, int ___numOfBullets, float ___damageMultiplier, ProjectileFactory ___PF, Gun ___myGun)
        {
            if (!MainClass.RandomShotsAffectOnShoot) return true;
            if (!DumpAmmoPassiveShootRandomPatch.CurrentlyRandomShooting.HasValue) return true;

            Vector2 v = -DumpAmmoPassiveShootRandomPatch.CurrentlyRandomShooting.Value.normalized;
            float num = -1f * ___spread / 2f;
            for (int i = 0; i < ___numOfBullets; i++)
            {
                float degrees = num + (float)i / (float)___numOfBullets * ___spread;
                Vector2 direction = v.Rotate(degrees);
                ___PF.SpawnProjectile(___myGun.GetProjectileRecipe(), direction, __instance.transform.position, ___damageMultiplier);
            }

            return false;
        }
    }
}
