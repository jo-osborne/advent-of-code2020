using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {

        private static List<int> example01 = new List<int>
        {
            16,
            10,
            15,
            5,
            1,
            11,
            7,
            19,
            6,
            12,
            4
        };

        private static List<int> example02 = new List<int>
        {
            28,
            33,
            18,
            42,
            31,
            14,
            46,
            20,
            48,
            47,
            24,
            23,
            49,
            45,
            19,
            38,
            39,
            11,
            1,
            32,
            25,
            35,
            8,
            17,
            7,
            9,
            4,
            2,
            34,
            10,
            3,
        };

        private static int CalculateJolts(List<int> adapters)
        {
            adapters.Add(0);

            adapters.Sort();

            var countOneJolt = 0;
            var countThreeJolt = 0;

            for (var i = 1; i < adapters.Count; i++)
            {
                var diff = adapters[i] - adapters[i - 1];

                if (diff == 1)
                {
                    countOneJolt++;
                }
                if (diff == 3)
                {
                    countThreeJolt++;
                }
            }

            // include the built in adapter
            countThreeJolt++;

            Console.WriteLine($"one jolts = {countOneJolt} and three jolts = {countThreeJolt}");

            return countThreeJolt * countOneJolt;
        }

        private static long CalculatePaths(List<int> adapters)
        {
            adapters.Reverse();
            var downstreamJolts = new List<long> { 1 };

            for (var i = 1; i < adapters.Count; i++)
            {
                var adapter = adapters[i];
                var possibleConnectors = new List<int> { adapter + 1, adapter + 2, adapter + 3 };
                var actualConnectorIdxes = possibleConnectors.Where(c => adapters.Contains(c)).Select(c => adapters.IndexOf(c));
                var sum = 0L;
                foreach (var idx in actualConnectorIdxes)
                {
                    sum += downstreamJolts[idx];
                }
                downstreamJolts.Add(sum);
            }

            return downstreamJolts.Last();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(CalculateJolts(example01));
            Console.WriteLine(CalculateJolts(example02));

            var input = File.ReadAllLines("data.txt").Select(l => int.Parse(l)).ToList();
            Console.WriteLine(CalculateJolts(input));

            Console.WriteLine("Puzzle 02 - number of paths");
            Console.WriteLine(CalculatePaths(input));
        }
    }
}
