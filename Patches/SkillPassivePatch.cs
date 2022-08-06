using flanne.CharacterPassives;
using HarmonyLib;
using UnityEngine.InputSystem;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(SkillPassive), "Update")]
    public static class SkillPassiveUpdatePatch
    {
        public static void Postfix(SkillPassive __instance, InputAction ____skillAction, float ____timer)
        {
            if (____timer > 0 || !MainClass.HoldToActivateSkill || !____skillAction.IsPressed()) return;
            Traverse.Create(__instance).Method("PerformSkillCallback", default(InputAction.CallbackContext)).GetValue(default(InputAction.CallbackContext));
        }
    }
}
