using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("data.txt");
            var numbers = input.Select(int.Parse).ToList();
            numbers.Sort();

            var answer1 = Puzzle01(numbers);
            var answer2 = Puzzle02(numbers);

            Console.WriteLine($"Puzzle 1:\n\t{answer1}\n\nPuzzle 2:\n\t{answer2}");
        }

        record Answer(List<int> Values, int Product);

        static Answer Puzzle01(List<int> sortedNumbers)
        {
            return FindNumbersSumming(2020, sortedNumbers);
        }

        static Answer Puzzle02(List<int> sortedNumbers)
        {
            foreach (var number in sortedNumbers)
            {
                var target = 2020 - number;

                try
                {
                    var result = FindNumbersSumming(target, sortedNumbers);
                    return new Answer(result.Values.Append(number).ToList(), result.Product * number);
                }
                catch (Exception)
                {
                }
            }

            throw new Exception("No result found");
        }

        // Assumes numbers are sorted
        private static Answer FindNumbersSumming(int target, List<int> numbers)
        {
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
                        return new Answer(new List<int>() { innerNumber, outerNumber }, innerNumber * outerNumber);
                    }

                    innerIdx++;
                }
            }
            
            throw new Exception($"No valid result found for target {target}");
        }
    }
}
