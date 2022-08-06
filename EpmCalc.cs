using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItrsTweaks
{
    public class EpmCalc
    {
        public struct E : IComparable<E>
        {
            public float Time;
            public int Count;

            public E(float time, int count) : this()
            {
                Time = time;
                Count = count;
            }

            public int CompareTo(E other)
            {
                return Time.CompareTo(other.Time);
            }

            public static bool operator <(E a, E b)
            {
                return a.Time < b.Time;
            }
            public static bool operator >(E a, E b)
            {
                return a.Time > b.Time;
            }
        }

        // Temporary string for GuiLayout until I make proper UI
        public string Prefix { get; private set; }
        public string GuiLayoutString { get; private set; }
        public int EventsPerSecond { get; private set; }
        public int EventsPerMinute { get; private set; }

        private float _updateTimer;
        private List<E> _events = new List<E>();
        private bool _preferPerSecond;
        private int _preferPerSecondCounter;

        private int _sum = 0;

        public EpmCalc(string prefix)
        {
            Prefix = prefix;
        }

        public void OnEvent(int count = 1)
        {
            var eventAt = Time.fixedTime;
            _events.Add(new E(eventAt, count));
            _sum += count;
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
            var timeFrom = new E(Time.fixedTime - 60, 0);
            if (_events.Count > 0 && _events[0] < timeFrom)
            {
                var removeIndex = _events.BinarySearch(timeFrom);
                if (removeIndex < 0) removeIndex = ~removeIndex;

                for (var i = 0; i < removeIndex; i++)
                {
                    _sum -= _events[i].Count;
                }
                _events.RemoveRange(0, removeIndex);
            }

            if (_events.Count <= 1)
            {
                EventsPerMinute = 0;
                EventsPerSecond = 0;
                return;
            }
            var fiveSecondsAgo = new E(Time.fixedTime - 5, 0);
            var lastEvent = _events[_events.Count - 1];

            EventsPerMinute = _sum;

            if (lastEvent < fiveSecondsAgo)
            {
                EventsPerSecond = 0;
                return;
            }

            var fiveSecAgoIndex = _events.BinarySearch(fiveSecondsAgo);
            if (fiveSecAgoIndex < 0) fiveSecAgoIndex = ~fiveSecAgoIndex;

            EventsPerSecond = 0;
            for (var i = fiveSecAgoIndex; i < _events.Count; i++)
            {
                EventsPerSecond += i;
            }
        }

        public void UpdateUi()
        {
            if (EventsPerSecond < 5)
            {
                _preferPerSecondCounter = 0;
                _preferPerSecond = false;
            }
            else if (EventsPerSecond >= 20)
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
