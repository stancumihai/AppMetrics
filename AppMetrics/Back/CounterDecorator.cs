// ReSharper disable once CheckNamespace

using System;
using ConsoleApp1.meters;

namespace ConsoleApp1
{
    public static class CounterDecorator
    {

        public static void Wrapper(Func<int,int> functionToPass)
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            int length = functionToPass.Method.ToString().Length;
            Counter counter = metricsRegistry.Counter(functionToPass.Method.ToString().Substring(6, length - 13));
            using (counter.count())
            {
                functionToPass(1);
            }
        }
    }
}