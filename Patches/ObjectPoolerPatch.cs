using flanne;
using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;
using UnityEngine;

namespace ItrsTweaks
{
    [HarmonyPatch(typeof(ObjectPooler), "Awake")]
    public static class ObjectPoolerAwakePatch
    {
        /* Handles:
            0: BulletImpact, EnemyDeathFX, SmallXP, LargeXP, Chest, DevilDeal, ExplosionFX, 
            7: Burn, FireExplosionFireExplosionSmallDamagePopup, BrainMonster, Boomer, ElderBrain, 
            12: EyeMonster, EyeMonsterProjectileLamprey, WingedMonsterSpawnerMonsterGrenadeExplosion
        */
        public const int BulletImpact = 0;
        public const int SmallXp = 2;
        public const int LargeXp = 3;
        public const int ExplosionFx = 4;
        public const int Burn = 7;

        private static Transform prefabParent;

        public static void Prefix(ObjectPooler __instance)
        {
            var newItemsToPool = new List<ObjectPoolItem>(__instance.itemsToPool);
            __instance.itemsToPool = newItemsToPool;

            prefabParent = new GameObject("::ReplacedPrefabParent").transform;
            prefabParent.gameObject.SetActive(false);

            ReplacePrefab(BulletImpact, MainClass.HideBullets);
            ReplacePrefab(ExplosionFx, MainClass.HideBullets);
            ReplacePrefab(SmallXp, MainClass.HideXp);
            ReplacePrefab(LargeXp, MainClass.HideXp);

            void ReplacePrefab(
                int index,
                MelonPreferences_Entry<bool> entry
            )
            {
                var toModify = newItemsToPool[index].objectToPool;
                var disableGraphics = toModify.GetComponent<DisableGraphics>();

                if (disableGraphics == null)
                {
                    MainClass.Msg($"Replacing prefab {toModify.name}");

                    toModify = Object.Instantiate(toModify, prefabParent);
                    toModify.name = toModify.name.Replace("(Clone)", "(Replaced)");
                    disableGraphics = toModify.AddComponent<DisableGraphics>();

                    var item = newItemsToPool[index];
                    newItemsToPool[index] = new ObjectPoolItem(
                        item.tag, toModify, item.amountToPool, item.shouldExpand
                    );
                }

                disableGraphics.PreferenceId = entry.Identifier;
                entry.OnValueChanged += disableGraphics.OnValueChanged;

                var components = toModify.GetComponentsInChildren<Renderer>(true);
                if (components.Length == 0)
                {
                    MainClass.Error("No sprite renderers");
                    GameObject.Destroy(disableGraphics);
                    return;
                }

                disableGraphics.ToDisable = components;
                for (var i = 0; i < components.Length; i++)
                {
                    MainClass.Msg($"Disabling renderer: {components[i].name}");
                    components[i].enabled = !entry.Value;
                }
            }
        }
    }

    [HarmonyPatch(typeof(ObjectPooler), "AddObject")]
    public static class ObjectPoolerAddObjectPatch
    {
        public static readonly HashSet<string> BulletNames = new HashSet<string>();

        public static void Prefix(string tag, ref GameObject GO)
        {
            if (!BulletNames.Contains(tag)) return;

            var toModify = GO;
            var disableGraphics = toModify.GetComponent<DisableGraphics>();

            if (disableGraphics == null)
            {
                MainClass.Msg($"Replacing bullet prefab {toModify.name}");

                var prefabParent = new GameObject("::ReplacedBulletPrefabParent").transform;
                prefabParent.gameObject.SetActive(false);

                toModify = Object.Instantiate(toModify, prefabParent);
                toModify.name = toModify.name.Replace("(Clone)", "(Replaced)");
                disableGraphics = toModify.AddComponent<DisableGraphics>();
            }
            else
            {
                MainClass.Error("Somehow tried to modify already existing bullet");
            }

            var entry = MainClass.HideBullets;
            disableGraphics.PreferenceId = entry.Identifier;
            entry.OnValueChanged += disableGraphics.OnValueChanged;

            var components = toModify.GetComponentsInChildren<Renderer>(true);
            disableGraphics.ToDisable = components;
            for (var i = 0; i < components.Length; i++)
            {
                MainClass.Msg($"Disabling gun renderer: {components[i].name}");
                components[i].enabled = !MainClass.HideBullets.Value;
            }

            GO = toModify;
        }
    }
}
