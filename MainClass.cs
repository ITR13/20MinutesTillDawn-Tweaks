using System;
using MelonLoader;
using ItrsTweaks;

[assembly: MelonInfo(typeof(MainClass), "ITR's Tweaks", "0.1.0", "ITR")]
[assembly: MelonGame("Flanne", "MinutesTillDawn")]
namespace ItrsTweaks
{
    public class MainClass : MelonMod
    {
        public static Action<string> Msg { get; private set; }
        public static Action<string> Warning { get; private set; }
        public static Action<string> Error { get; private set; }

        private static MelonPreferences_Entry<int> _volumePercent;
        private static MelonPreferences_Entry<bool> _holdToActivateSkill;
        private static MelonPreferences_Entry<bool> _noAttack;

        public static int VolumePercent => _volumePercent.Value;
        public static bool HoldToActivateSkill => _holdToActivateSkill.Value;
        public static bool RegularAttackAllowed => !_noAttack.Value;

        public override void OnApplicationStart()
        {
            Msg = LoggerInstance.Msg;
            Warning = LoggerInstance.Warning;
            Error = LoggerInstance.Error;

            var category = MelonPreferences.CreateCategory("ItrsTweaks");

            _volumePercent = category.CreateEntry("VolumeStep", 10, "Volume Step (Percent)", "How many percent the volume slider steps every time you click it");
            _holdToActivateSkill = category.CreateEntry("HoldToActivateSkill", true, "Automatic Skill", "Allows you to hold right click to automatically fire your skill");

            var challengesCategory = MelonPreferences.CreateCategory("ItrsChallenges");

            _noAttack = challengesCategory.CreateEntry("NoAttack", false, "Disable Main Attack", "Prevents you from attacking and reloading with your left mouse button");
        }
    }
}