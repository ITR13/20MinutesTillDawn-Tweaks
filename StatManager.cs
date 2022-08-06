using System.Collections.Generic;
using UnityEngine;

namespace ItrsTweaks
{
    public static class StatManager
    {
        private static EpmCalc _burnsPerSecond = new EpmCalc("Burns");
        private static EpmCalc _freezesPerSecond = new EpmCalc("Freezes");
        private static EpmCalc _thundersPerSecond = new EpmCalc("Thunder hits");
        private static EpmCalc _damagePerSecond = new EpmCalc("Damage");

        private static int _lastShieldTime;
        private static List<float> _shieldTimers = new List<float>();
        private static string _timeUntilShield;

        private static int _lastHealTime;
        private static List<float> _healTimers = new List<float>();
        private static string _timeUntilHeal;

        #region GUI stuff
        private static int _controlId = -1;
        private static Rect _guiRect = new Rect(20, 200, 200, 30 * 6);
        #endregion

        public static void OnFixedUpdate()
        {
            CalcTime("Shield regen: ", _shieldTimers, ref _lastShieldTime, ref _timeUntilShield);
            CalcTime("Health drop: ", _healTimers, ref _lastHealTime, ref _timeUntilHeal);
            _burnsPerSecond.OnFixedUpdate();
            _freezesPerSecond.OnFixedUpdate();
            _thundersPerSecond.OnFixedUpdate();
            _damagePerSecond.OnFixedUpdate();

            _guiRect.width = 200;
            _guiRect.height = 30 * 6;
        }


        internal static void OnGUI()
        {
            if (_controlId == -1)
            {
                _controlId = GUIUtility.GetControlID(FocusType.Passive);
            }

            _guiRect = GUILayout.Window(_controlId, _guiRect, GuiWindow, "Statistics");
        }

        private static void GuiWindow(int id)
        {
            GUI.DragWindow();
            if (!string.IsNullOrEmpty(_timeUntilShield)) GUILayout.Label(_timeUntilShield);
            if (!string.IsNullOrEmpty(_timeUntilHeal)) GUILayout.Label(_timeUntilHeal);
            if (!string.IsNullOrEmpty(_burnsPerSecond.GuiLayoutString)) GUILayout.Label(_burnsPerSecond.GuiLayoutString);
            if (!string.IsNullOrEmpty(_freezesPerSecond.GuiLayoutString)) GUILayout.Label(_freezesPerSecond.GuiLayoutString);
            if (!string.IsNullOrEmpty(_thundersPerSecond.GuiLayoutString)) GUILayout.Label(_thundersPerSecond.GuiLayoutString);
            if (!string.IsNullOrEmpty(_damagePerSecond.GuiLayoutString)) GUILayout.Label(_damagePerSecond.GuiLayoutString);
        }

        public static void AddShieldCooldown(float time)
        {
            _shieldTimers.Add(time);
        }
        public static void AddHealCooldown(float time)
        {
            _healTimers.Add(time);
        }

        private static void CalcTime(string prefix, List<float> timers, ref int lastTime, ref string guiString)
        {
            if (timers.Count == 0)
            {
                guiString = "";
                lastTime = 0;
                return;
            }

            for (int i = timers.Count - 1; i >= 0; i--)
            {
                timers[i] -= Time.fixedDeltaTime;

                var intTime = Mathf.CeilToInt(timers[i]);
                if (intTime < lastTime || lastTime == 0)
                {
                    lastTime = intTime;
                    guiString = $"{prefix}: {intTime}s";
                }

                if (intTime <= 0)
                {
                    timers.RemoveAt(i);
                }
            }
        }

        public static void OnFire(object _, object __)
        {
            _burnsPerSecond.OnEvent(1);
        }

        public static void OnFreeze(object arg1, object arg2)
        {
            _freezesPerSecond.OnEvent(1);
        }

        public static void OnThunderHit(object arg1, object arg2)
        {
            _thundersPerSecond.OnEvent(1);
        }

        public static void OnDamage(int change)
        {
            _damagePerSecond.OnEvent(change);
        }
    }
}
