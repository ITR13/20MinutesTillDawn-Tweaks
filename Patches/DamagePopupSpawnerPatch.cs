using flanne;
using HarmonyLib;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(DamagePopupSpawner), "OnDamageTaken")]
    public class DamagePopupSpawnerOnDamageTakenPatch
    {
        public static bool Prefix(int amount)
        {
            StatManager.OnDamage(-amount);
            return MainClass.ShowDamageNumbers;
        }
    }
}
