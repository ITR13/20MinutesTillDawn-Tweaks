using System;
using UnityEngine;
using UnityEngine.Events;

namespace ItrsTweaks
{
    public class DisableGraphics : MonoBehaviour
    {
        [Serializable]
        public class MyEvent : UnityEvent<DisableGraphics> { }

        [SerializeField]
        public Renderer[] ToDisable;

        [SerializeField]
        public Light[] LightsToDisable;

        [SerializeField]
        public string PreferenceId;

        private void Awake()
        {
            if (string.IsNullOrEmpty(PreferenceId)) return;
            MainClass.BoolPreferences[PreferenceId].OnValueChanged += OnValueChanged;
        }

        private void OnDestroy()
        {
            MainClass.BoolPreferences[PreferenceId].OnValueChanged -= OnValueChanged;
        }

        public void OnValueChanged(bool from, bool to)
        {
            if (from == to) return;

            for (var i = 0; i < ToDisable.Length; i++)
            {
                ToDisable[i].enabled = !to;
            }
        }
    }
}
