using System.Collections.Generic;
using UnityEngine;

namespace ItrsTweaks
{
    public class EpmCalc
    {
        // Temporary string for GuiLayout until I make proper UI
        public string Prefix { get; private set; }
        public string GuiLayoutString { get; private set; }
        public int EventsPerSecond { get; private set; }
        public int EventsPerMinute { get; private set; }

        private float _updateTimer;
        private List<float> _events = new List<float>();
        private bool _preferPerSecond;
        private int _preferPerSecondCounter;

        public EpmCalc(string prefix)
        {
            Prefix = prefix;
        }

        public void OnEvent(int count = 1)
        {
            var eventAt = Time.fixedTime;
            for (var i = 0; i < count; i++)
            {
                _events.Add(eventAt);
            }
        }

        public void OnFixedUpdate()
        {
            _updateTimer += Time.fixedDeltaTime;
            if (_updateTimer < 1) return;
            _updateTimer -= 1;

            UpdateCount();
            UpdateUi();
        }

        public void UpdateCount()
        {
            var timeFrom = Time.fixedTime - 60;
            if (_events.Count > 0 && _events[0] < timeFrom)
            {
                var removeIndex = _events.BinarySearch(timeFrom);
                if (removeIndex < 0) removeIndex = ~removeIndex;
                _events.RemoveRange(0, removeIndex);
            }

            if (_events.Count <= 1)
            {
                EventsPerMinute = 0;
                EventsPerSecond = 0;
                return;
            }
            var fiveSecondsAgo = Time.fixedTime - 5;
            var lastEvent = _events[_events.Count - 1];

            if (lastEvent < Time.fixedTime - 20)
            {
                _events.Clear();
                EventsPerMinute = 0;
                EventsPerSecond = 0;
                return;
            }

            EventsPerMinute = _events.Count;

            if (lastEvent < fiveSecondsAgo)
            {
                EventsPerSecond = 0;
                return;
            }

            var fiveSecAgoIndex = _events.BinarySearch(fiveSecondsAgo);
            if (fiveSecAgoIndex < 0) fiveSecAgoIndex = ~fiveSecAgoIndex;

            EventsPerSecond = (_events.Count - fiveSecAgoIndex) / 5;
        }

        public void UpdateUi()
        {
            if (EventsPerSecond >= 20)
            {
                _preferPerSecondCounter++;
                if (_preferPerSecondCounter >= 10)
                {
                    _preferPerSecondCounter = 10;
                    _preferPerSecond = true;
                }
            }
            else
            {
                _preferPerSecondCounter--;
                if (_preferPerSecondCounter <= 0)
                {
                    _preferPerSecondCounter = 0;
                    _preferPerSecond = false;
                }
            }

            if (EventsPerMinute == 0)
            {
                GuiLayoutString = "";
                return;
            }

            if (_preferPerSecond)
            {
                GuiLayoutString = $"{Prefix}: {EventsPerSecond}/s";
            }
            else
            {
                GuiLayoutString = $"{Prefix}: {EventsPerMinute}/m";
            }
        }
    }
}
