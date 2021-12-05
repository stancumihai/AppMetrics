using System;
using ConsoleApp1.meters;

// ReSharper disable All

namespace ConsoleApp1.reporters
{
    public class ConsoleReporter
    {
        public void Report()
        {
            MetricsRegistry mReg = MetricsRegistry.Instance;
            foreach (var key in mReg.Keys)
            {
                // pentru fiecare store imi face o cheie si devine 0 oricare min max ... parametru
                Store hist = hist = mReg.Timering(key).Store;
                Console.WriteLine("===========================================");
                Console.WriteLine("Report for: " + key);
                Console.WriteLine("Number of calls: " + hist.Counter); // momentan e 0
                Console.WriteLine("Avg execution time: " + hist.GetMean()); // momentan e 0
                Console.WriteLine("Total execution time: " + hist.Sum); 
                Console.WriteLine("Max execution time: " + hist.Max);
                Console.WriteLine("Min execution time: " + hist.Min);
            }
        }
    }
}