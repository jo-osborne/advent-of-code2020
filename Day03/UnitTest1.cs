using NUnit.Framework;
using System;
using System.IO;

namespace Day03
{
    public class Tests
    {
        private string[] exampleInput = new string[]
        {
            "..##.......",
            "#...#...#..",
            ".#....#..#.",
            "..#.#...#.#",
            ".#...##..#.",
            "..#.##.....",
            ".#.#.#....#",
            ".#........#",
            "#.##...#...",
            "#...##....#",
            ".#..#...#.#"
        };

        [Test]
        public void TestExample()
        {
            var terrain = new Terrain(exampleInput);
            var slope = new Slope(3, 1);
            var route = terrain.Traverse(slope);
            Assert.That(route.NumberOfTrees, Is.EqualTo(7));
        }

        [Test]
        public void TestAtEndOfTerrain()
        {
            var terrain = new Terrain(
                new string[]
                {
                    ".",
                    ".",
                    "."
                });

            Assert.That(terrain.AtEndOfTerrain(new Coords(0, 0)), Is.False);
            Assert.That(terrain.AtEndOfTerrain(new Coords(0, 1)), Is.False);
            Assert.That(terrain.AtEndOfTerrain(new Coords(0, 2)), Is.True);
        }

        [Test]
        public void TestTreesAtPosition()
        {
            var input = new string[]
            {
                "#...",
                ".#.."
            };

            var terrain = new Terrain(input);

            Assert.That(terrain.TreesAtPosition(new Coords(0, 0)), Is.EqualTo(1));
            Assert.That(terrain.TreesAtPosition(new Coords(1, 0)), Is.EqualTo(0));
            Assert.That(terrain.TreesAtPosition(new Coords(2, 0)), Is.EqualTo(0));
            Assert.That(terrain.TreesAtPosition(new Coords(3, 0)), Is.EqualTo(0));

            Assert.That(terrain.TreesAtPosition(new Coords(0, 1)), Is.EqualTo(0));
            Assert.That(terrain.TreesAtPosition(new Coords(1, 1)), Is.EqualTo(1));
            Assert.That(terrain.TreesAtPosition(new Coords(2, 1)), Is.EqualTo(0));
            Assert.That(terrain.TreesAtPosition(new Coords(3, 1)), Is.EqualTo(0));
        }

        [Test]
        public void TestMoveOneSlope()
        {
            var slope = new Slope(3, 1);

            var terrain = new Terrain(
                // our move should take us from A to B
                new string[]
                {
                    "A...",
                    "...B",
                    "....",
                });

            var position = new Coords(0, 0);

            Assert.That(terrain.Move(position, slope), Is.EqualTo(new Coords(3, 1)));
        }

        [Test]
        public void TestMoveOverlappingSlope()
        {
            var slope = new Slope(3, 1);

            var terrain = new Terrain(
                // our move should take us from A to B
                new string[]
                {
                    "..A.",
                    ".B..",
                    "....",
                });

            var position = new Coords(2, 0);
            
            Assert.That(terrain.Move(position, slope), Is.EqualTo(new Coords(1, 1)));
        }

        public class Terrain
        {
            private string[] _terrain;
            private int _terrainWidth;

            public Terrain(string[] terrain)
            {
                _terrain = terrain;
                _terrainWidth = terrain[0].Length;
            }

            public Route Traverse(Slope slope)
            {
                var position = new Coords(0, 0);
                var route = new Route(0);

                while (!AtEndOfTerrain(position))
                {
                    position = Move(position, slope);
                    route = new Route(route.NumberOfTrees + TreesAtPosition(position));
                }

                return route;
            }

            public bool AtEndOfTerrain(Coords position) => (position.Y == _terrain.Length - 1);

            public Coords Move(Coords position, Slope slope)
            {
                var newX = (position.X + slope.Right) % _terrainWidth;

                return new Coords(newX, position.Y + slope.Down);
            }

            public int TreesAtPosition(Coords position)
            {
                var line = _terrain[position.Y];
                var elem = line[position.X];
                return elem == '#' ? 1 : 0;
            }
        }

        public record Slope(int Right, int Down);

        public record Route(int NumberOfTrees);

        public record Coords(int X, int Y);

        [Test]
        public void Puzzle01()
        {
            var terrain = new Terrain(File.ReadAllLines("data.txt"));
            var route = terrain.Traverse(new Slope(3, 1));
            Console.WriteLine($"Puzzle One output: {route}");
            Assert.That(route.NumberOfTrees, Is.EqualTo(205));
        }

        [Test]
        public void Puzzle02()
        {
            var terrain = new Terrain(File.ReadAllLines("data.txt"));

            var routes = new Route[] 
            {
                terrain.Traverse(new Slope(1, 1)),
                terrain.Traverse(new Slope(3, 1)),
                terrain.Traverse(new Slope(5, 1)),
                terrain.Traverse(new Slope(7, 1)),
                terrain.Traverse(new Slope(1, 2))
            };

            var multiple = 1L;
            foreach (var route in routes)
            {
                multiple = multiple * route.NumberOfTrees;
            }

            Console.WriteLine(multiple);

            Assert.That(multiple, Is.EqualTo(3952146825L));
        }
    }
}