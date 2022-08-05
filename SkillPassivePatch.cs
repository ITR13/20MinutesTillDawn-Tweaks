using flanne;
using flanne.CharacterPassives;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(SkillPassive), "Update")]
    public class SkillPassiveStartPatch
    {
        public static void Postfix(SkillPassive __instance, InputAction ____skillAction, float ____timer)
        {
            if (____timer > 0 || !MainClass.HoldToActivateSkill || !____skillAction.IsPressed()) return;
            Traverse.Create(__instance).Method("PerformSkillCallback", default(InputAction.CallbackContext)).GetValue(default(InputAction.CallbackContext));
        }
    }
}
