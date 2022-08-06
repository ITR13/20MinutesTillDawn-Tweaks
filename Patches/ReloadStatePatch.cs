using flanne.Player;
using HarmonyLib;
using UnityEngine.InputSystem;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(ReloadState), "OnFireAction")]
    public class ReloadStateOnFireActionPatch
    {
        public static bool Prefix(InputAction.CallbackContext obj)
        {
            return MainClass.RegularAttackAllowed;
        }
    }
}
