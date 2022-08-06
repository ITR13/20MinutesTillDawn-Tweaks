using flanne;
using HarmonyLib;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(Health), "HPChange")]
    public static class HealthPatch
    {
        public static void Postfix(int change)
        {
            if (change < 0) StatManager.OnDamage(-change);
        }
    }
}
