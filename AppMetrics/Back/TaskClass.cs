using System;
using System.Threading;

namespace ConsoleApp1
{
    public class TaskClass
    {
        public static void TimerForAllTasks()
        {
            Measurer<int>.Wrapper(BubbleSort);
            // Measurer<int>.Wrapper(Task3, 1000);
            // Measurer<int>.Wrapper(Task3, 1000);
            // Measurer<int>.Wrapper(Task3, 1000);
            // Measurer<int>.Wrapper(Task4, 1000);
        }

        public static int BubbleSort()
        {
            int size = 100000;
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
            {
                var rand = new Random();
                arr[i] = rand.Next();
            }

            int temp;
            for (int j = 0; j <= arr.Length - 2; j++)
            {
                for (int i = 0; i <= arr.Length - 2; i++)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        temp = arr[i + 1];
                        arr[i + 1] = arr[i];
                        arr[i] = temp;
                    }
                }
            }

            return 1;
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

        public static int Task6()
        {
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

        public static int Task5()
        {
            Console.WriteLine("Ce imi place aici");
            Console.WriteLine("Ce imi place aici");
            Console.WriteLine("Ce imi place aici");
            Console.WriteLine("Ce imi place aici");
            Console.WriteLine("Ce imi place aici");

            return 1;
        }
    }
}