using System;
using System.Collections.Generic;
using System.IO;

namespace _01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var data = File.ReadAllLines("data.txt");

            var numbers = new List<int>();
            foreach (var line in data)
            {
                numbers.Add(int.Parse(line));
            }

            numbers.Sort();

            foreach (var number in numbers)
            {
                var target = 2020 - number;
                var result = FindNumbersSumming(target, numbers);
                if (result != (-1, -1))
                {
                    Console.WriteLine($"{number} * {result.Item1} * {result.Item2} = {number * result.Item1 * result.Item2}");
                    break;
                }
            }

            Console.ReadKey();
        }

        private static (int, int) FindNumbersSumming(int target, List<int> numbers)
        {
            var result = (-1, -1);

            for (int outerIdx = 0; outerIdx < numbers.Count; outerIdx++)
            {
                var outerNumber = numbers[outerIdx];

                var upper = target - outerNumber;

                var pair = numbers[outerIdx];

                var innerIdx = outerIdx + 1;
                while (pair <= upper && innerIdx < numbers.Count)
                {
                    var innerNumber = numbers[innerIdx];

                    if (innerNumber + outerNumber == target)
                    {
                        result = (innerNumber, outerNumber);
                        break;
                    }

                    innerIdx++;
                }

                if (result != (-1, -1))
                {
                    break;
                }


            }

            return result;
        }
    }
}
