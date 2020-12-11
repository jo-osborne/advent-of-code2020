using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Day07
{
    public class Tests
    {
        [Test]
        public void TestExampleCanGetPossibleContainingBags()
        {
            // dict of bag type -> list of possible container bag types
            var bagMap = new Dictionary<string, HashSet<string>> 
            {
                ["bright white"] = new HashSet<string> { "light red", "dark orange" },
                ["muted yellow"] = new HashSet<string> { "light red", "dark orange" },
                ["shiny gold"] = new HashSet<string> { "bright white", "muted yellow" },
                ["faded blue"] = new HashSet<string> { "muted yellow", "dark olive", "vibrant plum" },
                ["dark olive"] = new HashSet<string> { "shiny gold" },
                ["vibrant plum"] = new HashSet<string> { "shiny gold" },
                ["dotted black"] = new HashSet<string> { "dark olive", "vibrant plum" }
            };

            // start with the shiny gold bag
            var bag = "shiny gold";

            var ans = AllPossibleContainingBags(bag, bagMap);

            Assert.That(ans.Count, Is.EqualTo(4));
        }

        [Test]
        public void TestExampleCanParseBagRules()
        {
            var expectedBagMap = new Dictionary<string, HashSet<string>>
            {
                ["bright white"] = new HashSet<string> { "light red", "dark orange" },
                ["muted yellow"] = new HashSet<string> { "light red", "dark orange" },
                ["shiny gold"] = new HashSet<string> { "bright white", "muted yellow" },
                ["faded blue"] = new HashSet<string> { "muted yellow", "dark olive", "vibrant plum" },
                ["dark olive"] = new HashSet<string> { "shiny gold" },
                ["vibrant plum"] = new HashSet<string> { "shiny gold" },
                ["dotted black"] = new HashSet<string> { "dark olive", "vibrant plum" }
            };

            var rules = new List<string>
            {
                "light red bags contain 1 bright white bag, 2 muted yellow bags.",
                "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
                "bright white bags contain 1 shiny gold bag.",
                "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
                "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
                "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
                "faded blue bags contain no other bags.",
                "dotted black bags contain no other bags.",
            };

            var parsedBagMap = ParseRules(rules);

            Assert.That(parsedBagMap.Count, Is.EqualTo(expectedBagMap.Count));
            foreach (var item in expectedBagMap)
            {
                Assert.That(parsedBagMap.ContainsKey(item.Key));
                Assert.That(parsedBagMap[item.Key].Count, Is.EqualTo(item.Value.Count));
                foreach (var b in item.Value)
                {
                    Assert.That(parsedBagMap[item.Key].Contains(b));
                }
            }
        }

        [Test]
        public void Puzzle01()
        {
            var input = File.ReadAllLines("data.txt").ToList();
            var rules = ParseRules(input);
            var bagMap = AllPossibleContainingBags("shiny gold", rules);
            Assert.That(bagMap.Count, Is.EqualTo(121));
        }

        [Test]
        public void Puzzle02()
        {
            var rules = File.ReadAllLines("data.txt").ToList();

            //var rules = new List<string>
            //{
            //    "shiny gold bags contain 2 dark red bags.",
            //    "dark red bags contain 2 dark orange bags.",
            //    "dark orange bags contain 2 dark yellow bags.",
            //    "dark yellow bags contain 2 dark green bags.",
            //    "dark green bags contain 2 dark blue bags.",
            //    "dark blue bags contain 2 dark violet bags.",
            //    "dark violet bags contain no other bags."
            //};

            var bagMap = new Dictionary<string, Dictionary<string, int>>();

            foreach (var rule in rules)
            {
                //posh purple bags contain 4 bright lavender bags, 2 wavy chartreuse bags, 3 vibrant aqua bags.
                var splitStr = rule.Split(' ');
                var outerBag = string.Concat(splitStr[0], " ", splitStr[1]);
                var innerBags = new Dictionary<string, int>();
                var idx = 4;
                if (splitStr[idx] != "no")
                {
                    while (idx < splitStr.Length)
                    {
                        var count = int.Parse(splitStr[idx]);
                        var innerBag = string.Concat(splitStr[idx + 1], " ", splitStr[idx + 2]);
                        innerBags.Add(innerBag, count);
                        idx = idx + 4;
                    }
                }
                bagMap.Add(outerBag, innerBags);
            }

            var myBag = new Bag(bagMap, "shiny gold");

            Assert.That(myBag.BagCount, Is.EqualTo(3805));
        }

        class Bag
        {
            private Dictionary<string, Dictionary<string, int>> _bagMap;

            public Bag(Dictionary<string, Dictionary<string, int>> bagMap, string name)
            {
                _bagMap = bagMap;
                Name = name;
            }

            public string Name { get; private set; }
            public List<Bag> InnerBags
            {
                get
                {
                    var inner = _bagMap[Name];
                    var bags = new List<Bag>();
                    foreach (var b in inner)
                    {
                        for (var i=0; i<b.Value; i++)
                        {
                            bags.Add(new Bag(_bagMap, b.Key));
                        }
                    }
                    return bags;
                }
            }

            public int BagCount()
            {
                return InnerBags.Count + InnerBags.Sum(b => b.BagCount());
            }
        }

        private HashSet<string> AllPossibleContainingBags(string bag, Dictionary<string, HashSet<string>> bagMap)
        {
            var ans = bagMap[bag];

            var lastSize = -1;

            while (lastSize != ans.Count)
            {
                lastSize = ans.Count;
                var nextLevel = new HashSet<string>();
                foreach (var b in ans)
                {
                    var maybeNextLevel = (bagMap.ContainsKey(b)) ? bagMap[b] : new HashSet<string>();
                    nextLevel.UnionWith(maybeNextLevel);
                }
                ans.UnionWith(nextLevel);
            }

            return ans;
        }

        private Dictionary<string, HashSet<string>> ParseRules(List<string> rules)
        {
            var parsedBagMap = new Dictionary<string, HashSet<string>>();

            foreach (var rule in rules)
            {
                var splitStr = rule.Split(' ');
                var containingBag = string.Concat(splitStr[0], " ", splitStr[1]);

                var innerBags = new List<string>();
                var idx = 5;
                if (splitStr[idx] != "other")
                {
                    while (idx < splitStr.Length)
                    {
                        innerBags.Add(string.Concat(splitStr[idx], " ", splitStr[idx + 1]));
                        idx = idx + 4;
                    }
                }

                foreach (var inner in innerBags)
                {
                    if (parsedBagMap.ContainsKey(inner))
                        parsedBagMap[inner].Add(containingBag);
                    else
                        parsedBagMap[inner] = new HashSet<string>() { containingBag };
                }
            }

            return parsedBagMap;
        }
    }
}