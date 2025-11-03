/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static Semaphore semaphore = new Semaphore(0, 10);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            int initialState = 10;

            Thread firstThread = new Thread(DecrementAndPrint);
            firstThread.Start(initialState);
            firstThread.Join();

            ThreadPool.QueueUserWorkItem(DecrementAndPrintSemaphore, initialState);
            semaphore.WaitOne();


            Console.ReadLine();
        }

        static void DecrementAndPrint(object state)
        {
            int value = (int)state;

            if (value > 0)
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Value = {value}");
                value--;

                Thread newThread = new Thread(DecrementAndPrint);
                newThread.Start(value);

                newThread.Join();
            }
        }

        static void DecrementAndPrintSemaphore(object state)
        {
            int value = (int)state;

            if (value > 0)
            {
                Console.WriteLine($"ThreadPool {Thread.CurrentThread.ManagedThreadId}: Value = {value}");
                value--;

                ThreadPool.QueueUserWorkItem(DecrementAndPrintSemaphore, value);
            }
            else
            {
                semaphore.Release();
            }
        }
    }
}
