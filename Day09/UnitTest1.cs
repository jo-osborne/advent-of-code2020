using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day09
{
    public class Tests
    {
        [Test]
        public void ExampleTest()
        {
            var answer = -1L;

            var numbers = new long[]
            {
                35L,
                20L,
                15L,
                25L,
                47L,
                40L,
                62L,
                55L,
                65L,
                95L,
                102L,
                117L,
                150L,
                182L,
                127L,
                219L,
                299L,
                277L,
                309L,
                576L
            };

            var preambleLength = 5;

            answer = FindFirstException(numbers, preambleLength);

            Assert.That(answer, Is.EqualTo(127));
        }

        [Test]
        public void ExampleTest2()
        {
            var numbers = new long[]
            {
                35L,
                20L,
                15L,
                25L,
                47L,
                40L,
                62L,
                55L,
                65L,
                95L,
                102L,
                117L,
                150L,
                182L,
                127L,
                219L,
                299L,
                277L,
                309L,
                576L
            };

            var answer = BreakCode(numbers, 5);

            Assert.That(answer, Is.EqualTo(62));
        }

        [Test]
        public void Puzzle01()
        {
            var input = File.ReadAllLines("data.txt").Select(l => long.Parse(l)).ToArray();
            var result = FindFirstException(input, 25);

            Assert.That(result, Is.EqualTo(257342611));

            var answer = BreakCode(input, 25);
            Assert.That(answer, Is.EqualTo(35602097));
        }

        private long BreakCode(long[] numbers, int length)
        {
            var target = FindFirstException(numbers, length);

            var answer = -1L;

            for (var i = 0; i < numbers.Length; i++)
            {
                if (answer != -1) break;

                var j = i;
                var sum = 0L;

                while (j < numbers.Length && sum <= target)
                {
                    sum = sum + numbers[j];
                    if (sum == target && i != j)
                    {
                        var range = new ArraySegment<long>(numbers, i, j - i).ToArray();
                        Array.Sort(range);
                        var smallest = range[0];
                        var largest = range.Last();
                        answer = smallest + largest;
                        break;
                    }
                    j++;
                }
            }

            return answer;
        }
        private long FindFirstException(long[] numbers, int preambleLength)
        {
            var preamble = new long[preambleLength];
            var idx = -1;
            var count = 0;

            foreach (var n in numbers)
            {
                idx = (idx + 1) % preambleLength;

                if (count < preambleLength)
                {
                    Console.WriteLine($"initial array filling - {n} goes in {idx}");
                    preamble[idx] = n;
                    count++;
                }
                else
                {
                    if (!IsSumOfTwo(preamble, n))
                    {
                        Console.WriteLine($"{n} is not the sum of two of {string.Join(",", preamble)}");
                        return n;
                    }
                    else
                    {
                        preamble[idx] = n;
                        Console.WriteLine($"preamble is now {string.Join(",", preamble)}");
                    }
                }
            }

            throw new Exception("Did not find first exception uh oh");
        }

        private bool IsSumOfTwo(long[] arr, long n)
        {
            Console.WriteLine($"checking if {n} is the sum of two of {string.Join(",", arr)}");
            foreach (var x in arr)
            {
                Console.WriteLine($"checking if {string.Join(",", arr)} contains the value {n - x}");
                if (arr.Contains(n - x)) return true;
            }

            return false;
        }
    }
}