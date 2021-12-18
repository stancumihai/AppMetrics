using System;
using System.Threading;

namespace ConsoleApp1
{
    public class TaskClass
    {
        public static void TimerForAllTasks()
        {
            Measurer<int>.Wrapper(Task2);
            Measurer<int>.Wrapper(Task3,1000);
            Measurer<int>.Wrapper(Task3,1000);
            Measurer<int>.Wrapper(Task3,1000);
            Measurer<int>.Wrapper(Task4,1000);
            // Measurer<int>.Wrapper(Task2);
            // Measurer<int>.Wrapper(Task3,1000);
            
        }

        public static int Task1()
        {
            Console.WriteLine("Ce imi place aici");
            Console.WriteLine("Ce imi place aici");
            Console.WriteLine("Ce imi place aici");
            Console.WriteLine("Ce imi place aici");
            Console.WriteLine("Ce imi place aici");
            
            return 1;
        }

        public static int Task5()
        {
            Console.WriteLine("Ce imi place aici");
            Console.WriteLine("Ce imi place aici");
            Console.WriteLine("Ce imi place aici");
            Console.WriteLine("Ce imi place aici");
            Console.WriteLine("Ce imi place aici");
            
            return 1;
        }
        
        public static int Task2()
        {
            Thread.Sleep(2000);
            return 1;
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