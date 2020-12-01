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

            // sorted ascending (?)
            numbers.Sort();

            var result = -1;

            for (int outerIdx = 0; outerIdx < numbers.Count; outerIdx++)
            {
                var outerNumber = numbers[outerIdx];
                
                var upper = 2020 - outerNumber;

                var pair = numbers[outerIdx];

                var innerIdx = outerIdx + 1;
                while (pair <= upper && innerIdx < numbers.Count)
                {
                    var innerNumber = numbers[innerIdx];

                    if (innerNumber + outerNumber == 2020)
                    {
                        result = innerNumber * outerNumber;
                        Console.WriteLine($"{innerNumber} * {outerNumber} = {result}");
                        break;
                    }

                    innerIdx++;
                }

                if (result != -1)
                {
                    break;
                }

                        
            }


            Console.ReadKey();

        }
    }
}
