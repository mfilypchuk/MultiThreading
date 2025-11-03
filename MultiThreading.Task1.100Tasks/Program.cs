/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    class Program
    {
        const int TaskAmount = 100;
        const int MaxIterationsCount = 1000;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();
            
            HundredTasks();

            Console.ReadLine();
        }

        static void HundredTasks()
        {
            var tasks = new Task[TaskAmount];

            for (int taskNumber = 0; taskNumber < TaskAmount; taskNumber++)
            {
                int currentTaskNumber = taskNumber;

                tasks[taskNumber] = Task.Run(() =>
                {
                    for (int iteration = 1; iteration <= MaxIterationsCount; iteration++)
                    {
                        Output(currentTaskNumber, iteration);
                    }
                });
            }
            Task.WhenAll(tasks).Wait();
        }

        static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}
