using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ConsoleApp1.meters;

// ReSharper disable once CheckNamespace
namespace ConsoleApp1
{
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class MetricsRegistry
    {
        private HashSet<String> _keys;
        private Dictionary<String, Counter> _counters;
        private Dictionary<String, Timer> _timers;
        private Stopwatch _time = new();

        private static MetricsRegistry _instance;

        private MetricsRegistry()
        {
            _keys = new HashSet<String>();
            _counters = new Dictionary<string, Counter>();
            _timers = new Dictionary<string, Timer>();
            _time.Start();
        }

        public static MetricsRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MetricsRegistry();
                }

                return _instance;
            }
        }

        public Counter Counter(String key)
        {
            if (!_keys.Contains(key))
            {
                _keys.Add(key);
                _counters[key] = new Counter();
            }

            Counter insertedCounter = null;
            foreach (var keyName in _counters.Keys)
            {
                if (keyName.Equals(key))
                {
                    insertedCounter = _counters[key];
                }
            }

            return insertedCounter;
        }

        public Timer Timer(String key)
        {
            if (!_keys.Contains(key))
            {
                _keys.Add(key);
                _timers[key] = new Timer();
            }

            Timer insertedTimer = null;
            foreach (var keyName in _timers.Keys)
            {
                if (keyName.Equals(key))
                {
                    insertedTimer = _timers[key];
                }
            }

            return insertedTimer;
        }

        //N-am idee ce face
        public Dictionary<String, double> GetMetrics(Object key)
        {
            Dictionary<String, double> metrics = new();
            GetTimerMetrics((Timer)key);
            GetCounterMetrics((Counter)key);
            return null;
        }

        public Dictionary<String, double> GetTimerMetrics(Timer key)
        {
            if (!_timers.ContainsValue(key)) return null;
            Timer myTimer = null;

            foreach (var timer in _timers.Values.Where(timer => timer.Equals(key)))
            {
                myTimer = timer;
            }

            if (myTimer != null)
            {
                Dictionary<String, double> list = new()
                {
                    { "avg", myTimer.Get_Mean() },
                    { "sum", myTimer.Get_Sum() },
                    { "max", myTimer.Max },
                    { "min", myTimer.Min }
                };

                return list;
            }

            return null;
        }

        public Dictionary<String, double> GetCounterMetrics(Counter key)
        {
            if (!_counters.ContainsValue(key)) return null;
            Counter myCounter = null;
            foreach (var counter in _counters.Values.Where(counter => counter.Equals(key)))
            {
                myCounter = counter;
            }

            if (myCounter == null) return null;
            Dictionary<String, double> list = new() { { "count", myCounter.get_count() } };
            return list;
        }

        public HashSet<String> Keys
        {
            get => _keys;
            set => _keys = value;
        }

        public Dictionary<string, Counter> Counters
        {
            get => _counters;
            set => _counters = value;
        }

        public Dictionary<string, Timer> Timers
        {
            get => _timers;
            set => _timers = value;
        }

        public Counter Countering(String key)
        {
            return MetricsRegistry._instance.Counter(key);
        }

        public Timer Timering(String key)
        {
            return _instance.Timer(key);
        }
    }
}