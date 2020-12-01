using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("data.txt");
            var passwordDetails = input.Select(ParsePasswordDetails).ToList();

            var answer01 = ValidPasswords1(passwordDetails).Count();
            var answer02 = ValidPasswords2(passwordDetails).Count();

            Console.WriteLine($"Answer 1:\n\t{answer01}\n\nAnswer 2:\n\t{answer02}");
        }

        record PasswordDetails(int X, int Y, char Letter, string Password);

        static PasswordDetails ParsePasswordDetails(string item)
        {
            var x = int.Parse(item.Split('-')[0]);
            var y = int.Parse(item.Split('-')[1].Split(' ')[0]);
            var letter = char.Parse(item.Split(' ')[1].Split(':')[0]);
            var password = item.Split(' ')[2];

            return new PasswordDetails(x, y, letter, password);
        }

        static List<PasswordDetails> ValidPasswords1(List<PasswordDetails> passwords)
        {
            return passwords.Where(pwd =>
            {
                var numberOfOccurences = pwd.Password.Where(c => c == pwd.Letter).Count();
                return (numberOfOccurences >= pwd.X) && (numberOfOccurences <= pwd.Y);
            }).ToList();
        }

        static List<PasswordDetails> ValidPasswords2(List<PasswordDetails> passwords)
        {
            return passwords.Where(pwd =>
            {
                var password = pwd.Password;
                var firstPos = pwd.X;
                var secondPos = pwd.Y;
                var letter = pwd.Letter;
                return ((password[firstPos - 1] == letter) || (password[secondPos - 1] == letter)) && (password[firstPos - 1] != password[secondPos - 1]);
            }).ToList();
        }
    }
}
