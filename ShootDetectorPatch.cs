using flanne;
using HarmonyLib;
using UnityEngine.InputSystem;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(ShootDetector), "OnManualShoot")]
    public static class ShootDetectorOnManualShootPatch
    {
        public static bool Prefix(InputAction.CallbackContext context)
        {
            return MainClass.RegularAttackAllowed;
        }
    }
}
