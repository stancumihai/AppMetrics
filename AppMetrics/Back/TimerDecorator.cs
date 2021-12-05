using System;
using ConsoleApp1.meters;

namespace ConsoleApp1
{
    public static class TimerDecorator
    {
        public static Timer timer;

        public static Timer Timer
        {
            get => Timer;
            set => timer = Timer;
        }

        public static void Wrapper(Func<int, int> functionToPass, int size)
        {
            MetricsRegistry metricsRegistry = MetricsRegistry.Instance;
            int length = functionToPass.Method.ToString().Length;
            metricsRegistry.Timer(functionToPass.Method.ToString().Substring(6,length - 13));
            using (timer.time())
            {
                functionToPass(size);
            }
        }
    }
}