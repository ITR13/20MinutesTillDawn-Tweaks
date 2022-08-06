using flanne;
using HarmonyLib;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(DamagePopupSpawner), "OnDamageTaken")]
    public class DamagePopupSpawnerOnDamageTakenPatch
    {
        public static bool Prefix()
        {
            return MainClass.ShowDamageNumbers;
        }
    }
}
