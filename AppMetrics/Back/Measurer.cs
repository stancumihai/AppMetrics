using System;
using ConsoleApp1.meters;

namespace ConsoleApp1
{
    public static class Measurer<T>
    {
        public static void Wrapper(Func<T, T> functionToPass, T size)
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            Timer timer = metricsRegistry.Timer(functionToPass.Method.Name);
            using (timer.time())
            {
                functionToPass(size);
            }
        }

        public static void Wrapper(Func<T> functionToPass)
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            Timer timer = metricsRegistry.Timer(functionToPass.Method.Name);
            using (timer.time())
            {
                functionToPass();
            }
        }
    }
}