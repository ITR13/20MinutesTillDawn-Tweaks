using System;
using MelonLoader;
using VrcTracer;

[assembly: MelonInfo(typeof(MainClass), "ITR's Tweaks", "0.1.0", "ITR")]
[assembly: MelonGame("Flanne", "MinutesTillDawn")]
namespace VrcTracer
{
    public class MainClass : MelonMod
    {
        public static Action<string> Msg { get; private set; }
        public static Action<string> Warning { get; private set; }
        public static Action<string> Error { get; private set; }

        public override void OnApplicationStart()
        {
            Msg = LoggerInstance.Msg;
            Warning = LoggerInstance.Warning;
            Error = LoggerInstance.Error;
        }
    }
}