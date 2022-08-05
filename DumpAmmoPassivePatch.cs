using flanne;
using flanne.CharacterPassives;
using HarmonyLib;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(DumpAmmoPassive), "ShootRandom")]
    public static class DumpAmmoPassiveShootRandomPatch
    {
        public static Vector2? CurrentlyRandomShooting;

        private static Vector2 OldRandomDirection()
        {
            Vector2 vector = Vector2.zero;
            while (vector == Vector2.zero)
            {
                vector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            }
            return vector;
        }

        private static Vector2 NewRandomDirection()
        {
            var angle = Random.Range(0f, Mathf.PI * 2);
            return new Vector2(
                Mathf.Sin(angle),
                Mathf.Cos(angle)
            );
        }

        public static bool Prefix(Gun ___myGun, Shooter ___shooter)
        {
            var vector = MainClass.FixRandomShot ? NewRandomDirection() : OldRandomDirection();
            int num = ___myGun.shooters.Length;
            for (int i = 0; i < num; i++)
            {
                ___shooter.Shoot(___myGun.GetProjectileRecipe(), vector.Rotate(i * 10), ___myGun.numOfProjectiles, ___myGun.spread, 0f);
            }
            var gunshotSFX = ___myGun.gunData.gunshotSFX;
            // Null propagation doesn't work with unity's null
            if (gunshotSFX) gunshotSFX.Play();

            CurrentlyRandomShooting = vector;
            try
            {
                ___myGun.OnShoot.Invoke();
            }
            catch (Exception e)
            {
                MainClass.Eerror("Exception in OnShoot: ", e);
            }
            CurrentlyRandomShooting = null;

            return false;
        }
    }
}
