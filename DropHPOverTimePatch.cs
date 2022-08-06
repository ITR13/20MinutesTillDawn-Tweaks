using flanne;
using HarmonyLib;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(DropHPOverTime), "DropHP")]
    public static class DropHPOverTimeDropHPPatch
    {
        public static void Postfix(float ___timeToDrop)
        {
            StatManager.AddHealCooldown(___timeToDrop);
        }
    }
}
