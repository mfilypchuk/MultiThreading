/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var random = new Random();

            var firstTask = Task.Run(() =>
            {
                int[] randomIntegers = Enumerable.Range(1, 10).Select(_ => random.Next(1, 100)).ToArray();
                Console.WriteLine("First Task - Array of 10 random integers: " + string.Join(", ", randomIntegers));
                return randomIntegers;
            });

            var secondTask = firstTask.ContinueWith(previousTask =>
            {
                int[] array = previousTask.Result;
                int multiplier = random.Next(2, 10);
                int[] multipliedArray = array.Select(x => x * multiplier).ToArray();
                Console.WriteLine("Second Task - Array multiplied by " + multiplier + ": " + string.Join(", ", multipliedArray));
                return multipliedArray;
            });

            var thirdTask = secondTask.ContinueWith(previousTask =>
            {
                int[] array = previousTask.Result;
                Array.Sort(array);
                Console.WriteLine("Third Task - Array sorted in ascending order: " + string.Join(", ", array));
                return array;
            });

            var fourthTask = thirdTask.ContinueWith(previousTask =>
            {
                int[] array = previousTask.Result;
                double average = array.Average();
                Console.WriteLine("Fourth Task - Average value: " + average);
                return average;
            });

            Console.ReadLine();
        }
    }
}
