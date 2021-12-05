// ReSharper disable once CheckNamespace

using System;
using ConsoleApp1.meters;

namespace ConsoleApp1
{
    public static class CounterDecorator
    {
        public static Counter counter;

        public static Counter Counter
        {
            get => Counter;
            set => counter = Counter;
        }

        public static void Wrapper(Func<int,int> functionToPass)
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            int length = functionToPass.Method.ToString().Length;
            metricsRegistry.Counter(functionToPass.Method.ToString().Substring(6, length - 13));
            using (counter.count())
            {
                functionToPass(1);
            }
        }
    }
}