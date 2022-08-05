using flanne.Player;
using HarmonyLib;
using UnityEngine.InputSystem;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(IdleState), "OnFireAction")]
    public class IdleStateOnFireActionPatch
    {
        public static bool Prefix(InputAction.CallbackContext obj)
        {
            return MainClass.RegularAttackAllowed;
        }
    }
}
