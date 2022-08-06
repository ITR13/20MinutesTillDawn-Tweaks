using flanne;
using HarmonyLib;

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
