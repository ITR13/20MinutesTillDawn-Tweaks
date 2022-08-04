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

        public static MelonPreferences_Entry<int> _volumePercent;

        public static int VolumePercent => _volumePercent.Value;

        public override void OnApplicationStart()
        {
            Msg = LoggerInstance.Msg;
            Warning = LoggerInstance.Warning;
            Error = LoggerInstance.Error;

            var category = MelonPreferences.CreateCategory("ItrsTweaks");
            _volumePercent = category.CreateEntry("VolumeStep", 10, "Volume Step (Percent)", "How many percent the volume slider steps every time you click it");
        }
    }
}