using NUnit.Framework;
using System.IO;
using System.Linq;
using System;

namespace Day06
{
    public class Tests
    {
        [Test]
        public void Puzzle01()
        {
            var input = File.ReadAllText("data.txt");
            var groups = input.Split("\n\n");
            var answerCounts = groups.Select(g => g.Where(c => !Char.IsWhiteSpace(c))).Select(s => s.Distinct().Count());
            var sum = answerCounts.Sum();
            Console.WriteLine(sum);
            Assert.That(sum, Is.EqualTo(6703));
        }

        [Test]
        public void Puzzle02()
        {
            var input = File.ReadAllText("data.txt");
            var groups = input.Split("\n\n").Select(l => l.Split('\n'));

            var chars = Enumerable.Range('a', 26).Select(c => (char)c).ToList();
            var sum = 0;

            foreach (var group in groups)
            {
                foreach (var c in chars)
                {
                    var charIsInAllLines = true;
                    foreach (var line in group)
                    {
                        if (!line.Contains(c))
                        {
                            charIsInAllLines = false;
                        }
                    }
                    if (charIsInAllLines) sum++;
                }
            }

            Assert.That(sum, Is.EqualTo(0));
        }
    }
}