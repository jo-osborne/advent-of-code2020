using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace _04
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
            // either the char as pos 1 (x) OR the char at pos 9 (z) must be the specified char (x) [but NOT BOTH]
            // i.e. the above example is valid, because x is in pos 1

            var firstPos = int.Parse(item.Split('-')[0]);
            var secondPos = int.Parse(item.Split('-')[1].Split(' ')[0]);
            var letter = char.Parse(item.Split(' ')[1].Split(':')[0]);
            var password = item.Split(' ')[2];

            return ((password[firstPos - 1] == letter) || (password[secondPos - 1] == letter)) && (password[firstPos - 1] != password[secondPos - 1]);
            //return (password[firstPos - 1] == password[secondPos - 1]) && (password[firstPos - 1] == letter);

        }
    }
}
