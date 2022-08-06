using flanne;
using HarmonyLib;
using System.Collections.Generic;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(Gun), nameof(Gun.LoadGun))]
    public static class GunLoadGunPatch
    {
        public static void Prefix(GunData gunToLoad)
        {
            ObjectPoolerAddObjectPatch.BulletNames.Add(gunToLoad.bulletOPTag);
        }
    }
}
