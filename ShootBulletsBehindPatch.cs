using flanne;
using flanne.CharacterPassives;
using flanne.PowerupSystem;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(ShootBulletsBehind), "OnShoot")]
    public class ShootBHulletsBehindOnShootPatch
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
