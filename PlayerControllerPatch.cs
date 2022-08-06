using flanne;
using HarmonyLib;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(PlayerController), "Start")]
    public static class PlayerControllerStartPatch
    {
        public static void Postfix(PlayerController __instance)
        {
            __instance.AddObserver(StatManager.OnFire, BurnSystem.InflictBurnEvent);
            __instance.AddObserver(StatManager.OnFreeze, FreezeSystem.InflictFreezeEvent);
            __instance.AddObserver(StatManager.OnThunderHit, ThunderGenerator.ThunderHitEvent);
        }
    }
}
