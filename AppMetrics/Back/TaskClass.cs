using System.Threading;
using ConsoleApp1.meters;
using Timer = ConsoleApp1.meters.Timer;

namespace ConsoleApp1
{
    public class TaskClass
    {
        public static void CounterForAllTasks()
        {
            Counter counter = new Counter();
            CounterDecorator.counter = counter;
            CounterDecorator.Wrapper(Task1);
            CounterDecorator.Wrapper(Task1);
            CounterDecorator.Wrapper(Task1);
        }

        public static void TimerForAllTasks()
        {
            Timer timer = new Timer();
            TimerDecorator.timer = timer;
            TimerDecorator.Wrapper(Task1, 1000);
            TimerDecorator.Wrapper(Task2, 1000);
            TimerDecorator.Wrapper(Task3, 1000);
            TimerDecorator.Wrapper(Task4, 1000);
        }

        public static int Task1(int size)
        {
            Thread.Sleep(size);
            return size;
        }

        public static int Task2(int size)
        {
            Thread.Sleep(size);
            return size;
        }

        public static int Task3(int size)
        {
            Thread.Sleep(size);
            return size;
        }

        public static int Task4(int size)
        {
            Thread.Sleep(size);
            return size;
        }
    }
}