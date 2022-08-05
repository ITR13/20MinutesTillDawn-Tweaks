using flanne.Player;
using HarmonyLib;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(StateMachine), "CurrentState", MethodType.Setter)]
    public class StateMachineCurrentStatePatch
    {
        public static void Prefix(StateMachine __instance, ref State value)
        {
            // This should _only_ activate while reloading, all other cases should be handled by ReloadStatePatch and SkillPassivePatch
            if (!(value is ShootingState) || MainClass.RegularAttackAllowed) return;
            value = __instance.GetComponent<IdleState>();
            if (value != null) return;
            value = __instance.gameObject.AddComponent<IdleState>();
        }
    }
}
