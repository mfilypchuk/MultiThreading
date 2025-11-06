/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            Task taskA = Task.Run(() =>
            {
                Console.WriteLine("Task A started");
                Thread.Sleep(500);

                Console.WriteLine("Task A completed");
            });

            taskA.ContinueWith(t =>
            {
                Console.WriteLine("a. Continuation executed regardless of result. Status: " + t.Status);
            });

            Task taskB = Task.Run(() =>
            {
                Console.WriteLine("Task B started");
                Thread.Sleep(500);
                throw new Exception("Task B failed");
            });

            taskB.ContinueWith(t =>
            {
                Console.WriteLine("b. Continuation executed because parent did not succeed. Status: " + t.Status);
            }, TaskContinuationOptions.NotOnRanToCompletion);

            Task taskC = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Task C started");
                Thread.Sleep(500);
                throw new Exception("Task C failed");
            });

            taskC.ContinueWith(t =>
            {
                Console.WriteLine("c. Continuation executed on fail, reusing parent thread. Status: " + t.Status);
                Console.WriteLine("c. Is thread pool thread: " + Thread.CurrentThread.IsThreadPoolThread);
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            var cts = new CancellationTokenSource();
            Task taskD = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Task D started");
                Thread.Sleep(500);
                cts.Token.ThrowIfCancellationRequested();
                Console.WriteLine("Task D completed");
            }, cts.Token);

            taskD.ContinueWith(t =>
            {
                Console.WriteLine("d. Continuation executed on cancel, outside thread pool. Status: " + t.Status);
                Console.WriteLine("d. Is thread pool thread: " + Thread.CurrentThread.IsThreadPoolThread);
            }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

            Thread.Sleep(200);
            cts.Cancel();

            try
            {
                Task.WaitAll(taskA, taskB, taskC, taskD);
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("One or more tasks failed: " + ex.Flatten().Message);
            }

            Thread.Sleep(1000);

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}
