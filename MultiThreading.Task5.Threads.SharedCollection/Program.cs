/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static List<int> sharedCollection = new List<int>();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection, and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Task class for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var task1Flag = true;

            var task1 = Task.Run(() =>
            {
                while (sharedCollection.Count < 10)
                {
                    if (task1Flag)
                    {
                        var number = sharedCollection.Count + 1;
                        sharedCollection.Add(number);
                        Console.WriteLine($"Added {number} to the collection");
                        task1Flag = false;
                    }
                }

            });
            Task task2 = Task.Run(() =>
            {
                while (sharedCollection.Count <= 10)
                {
                    if (!task1Flag)
                    {
                        Console.WriteLine("[" + string.Join(", ", sharedCollection) + "]");
                        task1Flag = true;
                    }

                }

            });
            Task.WhenAll(task1, task2).Wait();

            Console.ReadLine();
        }
    }
}
