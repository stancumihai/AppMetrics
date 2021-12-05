// ReSharper disable All

using System;

namespace ConsoleApp1.meters
{
    public class Counter
    {
        public int counter;
        private readonly object _balanceLock = new();

        public Counter()
        {
            counter = 0;
        }

        public void inc(int val = 1)
        {
            lock (_balanceLock)
            {
                counter += val;
            }
        }

        public CounterContext count()
        {
            return new CounterContext(this);
        }

        public int get_count()
        {
            return counter;
        }

        public void clear()
        {
            lock (_balanceLock)
            {
                counter = 0;
            }
        }
    }

    public class CounterContext : IDisposable
    {
        public Counter counter;

        public CounterContext(Counter counter)
        {
            this.counter = counter;
        }


        public void Dispose()
        {
            counter.inc();
        }
    }
}