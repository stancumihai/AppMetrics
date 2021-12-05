using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable 414

// ReSharper disable once CheckNamespace
namespace ConsoleApp1.meters
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Timer
    {
        private Stopwatch clock = new();
        private double max;
        private double min;
        private List<int> times;
        private Store store = new();

        public Timer()
        {
            clock.Start();
            max = float.MinValue;
            min = float.MaxValue;
            times = new List<int>();
            store = new Store();
        }

        public TimeContext time()
        {
            return new TimeContext(this);
        }

        public void Update(double seconds)
        {
            if (seconds > 0)
            {
                store.Add(seconds);
            }
        }

        public Stopwatch Clock
        {
            get => clock;
            set => clock = value;
        }

        public double Max
        {
            get => store.Max;
            set => max = value;
        }

        public double Min
        {
            get => store.Min;
            set => min = value;
        }

        public List<int> Times
        {
            get => times;
            set => times = value;
        }

        public double Get_Sum()
        {
            return store.Sum;
        }

        public Store Store
        {
            get => store;
            set => store = value;
        }


        public double Get_Mean()
        {
            return store.GetMean();
        }
    }

    public class TimeContext : IDisposable
    {
        private Stopwatch _clock = new();
        private Timer _timer;
        private static int OVERFLOW = 9000000;

        public TimeContext(Timer timer)
        {
            _clock.Start();
            _timer = timer;
        }

        private void Stop()
        {
            _clock.Stop();
            long ticks = _clock.ElapsedTicks;
            double elapsed = 1000000000.0 * ticks / Stopwatch.Frequency - OVERFLOW;
            _timer.Update(elapsed);
        }

        public void Dispose()
        {
            Stop();
        }
    }
}