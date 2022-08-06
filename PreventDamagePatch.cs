using flanne;
using HarmonyLib;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(PreventDamage), "Start")]
    public static class PreventDamageStartPatch
    {
        public static void Postfix(PreventDamage __instance)
        {
            __instance.OnDamagePrevented.AddListener(() => StatManager.AddShieldCooldown(0.5f + __instance.finalCooldown));
        }
    }
}
