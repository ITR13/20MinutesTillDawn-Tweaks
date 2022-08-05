﻿using System;
using MelonLoader;
using ItrsTweaks;
using UnityEngine;
using MelonLoader.Preferences;

[assembly: MelonInfo(typeof(MainClass), "ITR's Tweaks", "0.1.0", "ITR")]
[assembly: MelonGame("Flanne", "MinutesTillDawn")]
namespace ItrsTweaks
{
    public class MainClass : MelonMod
    {
        public static Action<string> Msg { get; private set; }
        public static Action<string> Warning { get; private set; }
        public static Action<string> Error { get; private set; }
        public static Action<string, Exception> Eerror { get; private set; }

        private static MelonPreferences_Entry<int> _volumePercent;
        private static MelonPreferences_Entry<bool> _holdToActivateSkill;
        private static MelonPreferences_Entry<bool> _noAttack;
        private static MelonPreferences_Entry<bool> _fixRandomShot;
        private static MelonPreferences_Entry<bool> _randomShotsAffectOnShoot;
        private static MelonPreferences_Entry<float> _timeScale;

        public static int VolumePercent => _volumePercent.Value;
        public static bool HoldToActivateSkill => _holdToActivateSkill.Value;
        public static bool RegularAttackAllowed => !_noAttack.Value;
        public static bool FixRandomShot => _fixRandomShot.Value;
        public static bool RandomShotsAffectOnShoot => _randomShotsAffectOnShoot.Value;

        public override void OnApplicationStart()
        {
            Msg = LoggerInstance.Msg;
            Warning = LoggerInstance.Warning;
            Error = LoggerInstance.Error;
            Eerror = LoggerInstance.Error;

            var category = MelonPreferences.CreateCategory("ITR'sTweaks");

            _volumePercent = category.CreateEntry("VolumeStep", 10, "Volume Step (Percent)", "How many percent the volume slider steps every time you click it", false, false, new LargerThan0ValueValidator());
            _holdToActivateSkill = category.CreateEntry("HoldToActivateSkill", true, "Automatic Skill", "Allows you to hold right click to automatically fire your skill");
            _fixRandomShot = category.CreateEntry("FixRandomShot", true, "Fix Random Shots", "Makes Abby's passive less biased");

            var cheatsCategory = MelonPreferences.CreateCategory("ITR'sCheats");
            _timeScale = cheatsCategory.CreateEntry("TimeScale", 1f, "Time scale", "Overwrites how fast the game is when not paused", false, false, new LargerThan0ValueValidator());

            var challengesCategory = MelonPreferences.CreateCategory("ITR'sChallenges");

            _noAttack = challengesCategory.CreateEntry("NoAttack", false, "Disable Main Attack", "Prevents you from attacking and reloading with your left mouse button");
            _randomShotsAffectOnShoot = challengesCategory.CreateEntry("AbbyExtraChallenge", false, "Abby Extra Challenge", "Makes Abby's passive also affect powerups that fire every nth shot");
        }

        public override void OnLateUpdate()
        {
            var oldTimescale = Time.timeScale;
            var timescale = _timeScale.Value;
            if (oldTimescale != 0 && oldTimescale != timescale)
            {
                Time.timeScale = timescale;
            }
        }
    }
}