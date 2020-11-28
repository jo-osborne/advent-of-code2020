using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace _03
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var data = File.ReadAllLines("data.txt");

            var numberOfValidPasswords = ValidPasswords(data).Count;

            Console.WriteLine($"There are {numberOfValidPasswords} valid passwords");
        }

        static List<string> ValidPasswords(string[] inputData)
        {
            var valid = new List<string>();

            foreach (var item in inputData)
            {
                if (IsValidPassword(item))
                {
                    valid.Add(item);
                }
            }

            return valid;
        }

        static bool IsValidPassword(string item)
        {
            // e.g.
            // 1-9 x: xwjgxtmrzxzmkx

            var min = int.Parse(item.Split('-')[0]);
            var max = int.Parse(item.Split('-')[1].Split(' ')[0]);
            var letter = char.Parse(item.Split(' ')[1].Split(':')[0]);
            var password = item.Split(' ')[2];

            var numberOfOccurences = password.Where(c => c == letter).Count();

            return (numberOfOccurences >= min) && (numberOfOccurences <= max);
        }
    }
}
