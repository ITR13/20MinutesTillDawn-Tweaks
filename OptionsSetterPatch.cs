using flanne;
using flanne.UI;
using HarmonyLib;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(OptionsSetter), nameof(OptionsSetter.OnClickSFXVolume))]
    public class SfxVolumePatch
    {
        public static void Prefix(AudioManager ___AM)
        {
            ___AM.SFXVolume = ___AM.SFXVolume - 0.25f + MainClass.VolumePercent / 100f;
        }
    }

    [HarmonyPatch(typeof(OptionsSetter), nameof(OptionsSetter.OnClickBGMVolume))]
    public class BgmVolumePatch
    {
        public static void Prefix(AudioManager ___AM)
        {
            ___AM.MusicVolume = ___AM.MusicVolume - 0.25f + MainClass.VolumePercent / 100f;
        }
    }
}
